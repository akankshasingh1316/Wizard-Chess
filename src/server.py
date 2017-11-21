#!/usr/bin/env python
#------------------------------------------------------------------------------
# Objectives:
# 1. Make a webserver that hosts a game of chess.
#	 This requires being able to show the user the board.
# 2. Make a speech recognition client that can interpret commands to AN moves
# 3. Enhance the client to make calls to the webserver running chess.
#------------------------------------------------------------------------------
# Libraries:
# Inspired by https://github.com/niklasf/web-boardimage
# Reference http://python-chess.readthedocs.io/en/v0.16.2/core.html
from aiohttp import web
import asyncio
import chess
import chess.svg
import subprocess
import json
from datetime import datetime
#------------------------------------------------------------------------------
boardcss = '''
.square.light {             fill: #e6e6e6;                      }
.square.dark {              fill: #8c8c8c;                      }
.square.light.lastmove {    fill: #ced26b;                      }
.square.dark.lastmove {     fill: #aaa23b;                      }
.check {                    fill: url(#check_gradient);         }
'''

defaultcss = '''
.square.light {				fill: #ffce9e;						}
.square.dark {				fill: #d18b47;						}
.square.light.lastmove {	fill: #cdd16a;						}
.square.dark.lastmove {		fill: #aaa23b;						}
.check {					fill: url(#check_gradient);			}
'''


HOSTNAME = "thunder.cise.ufl.edu"
PORT = 7777


def main():
	start(HOSTNAME, PORT)

def start(host_addr, port_num):
	app = web.Application()
	chessgame = GameController()
	app.router.add_get("/", chessgame.get_root)
	app.router.add_get("/board.png", chessgame.get_board)
	app.router.add_post("/move", chessgame.post_move)
	app.router.add_post("/undo", chessgame.post_undo)
	web.run_app(app, port=port_num, host=host_addr)
	
def svg2png(raw_svg, width=400):
	# Converts raw_svg data (in xml format) to png bytes.
	ifile = "board.svg"
	ofile = "board.png"
	with open(ifile, "w") as f:
		f.write(raw_svg)
	print(datetime.now().strftime("[%d-%m-%y|%H:%M:%S]"))
	subprocess.call(['inkscape', '-z', '-f', ifile, '-w', str(width), '-j', '-e', ofile])
	with open(ofile, "rb") as f:
		raw_png = f.read()
	return raw_png
	
def svg2jpeg(raw_svg, quality=50):
	svg2png(raw_svg, 400)
	ifile = "board.png"
	ofile = "board.jpeg"
	subprocess.call(['convert', ifile, '-quality', str(quality), ofile])
	print("Converting " + ifile + " to " + ofile + " with quality=" + str(quality))
	with open(ofile, "rb") as f:
		raw_jpg = f.read()
	return raw_jpg


class GameController:
	
	# Initialize the game.
	def __init__(self):
		self.board = chess.Board()
		#self.css = boardcss
		self.css = defaultcss
		self.gamestate = {
			"turn": "white",
			"moves": "",
			"status": ""
		}
	
#----[Routes for the web application]------------------------------------------

	# Default route. Redirect to the board.
	@asyncio.coroutine
	def get_root(self, request):
		print("new_game")
		self.board = chess.Board()
		return web.HTTPFound("/board.png#newgame")
	
	# Responds to a GET request for the board.
	# Todo: Allow for parameters to alter the board.svg file?
	@asyncio.coroutine
	def get_board(self, request):
		print("get_board")
		return web.Response(body=self.render_board(request), content_type="image/jpeg")
	
	# Responds to a POST request for making a move.
	@asyncio.coroutine
	def post_move(self, request):
		print("post_move")
		data = yield from request.post()
		# Apply the move. If it is successful, redirect to getting the board.
		rv, moves = self.make_move(data)
		self.gamestate['status'] = moves
		return web.Response(status=rv, text=json.dumps(self.gamestate), content_type="text/json")
		
	@asyncio.coroutine
	def post_undo(self, request):
		print("undo_move")
		self.undo_move()
		return web.Response(text="Last move undone", content_type="text/plain")

#----[Game functions, called by routes]----------------------------------------
	
	# Returns the active board, in svg format.
	# Now returns the board in PNG format!
	def render_board(self, request):
		try:
			hint = (request.GET['hint'].lower() == 'true')
		except (KeyError, ValueError):
			hint = False
	
		next_moves = chess.SquareSet()
		if hint:
			for move in self.board.legal_moves:
				next_moves.add(move.to_square)
		# Conversion code:
		raw_svg = chess.svg.board(board=self.board, squares=next_moves, style=self.css)
		return svg2jpeg(raw_svg)
		
		
	# Applies a post request move to the board. Returns status code if move invalid.
	def make_move(self, post_data):
		# If the post_data looks weird, it's because it's url encoded.
		# First, validate the paramters we need to make a move.
		try:
			piece = post_data["piece"].upper()
		except (KeyError, ValueError):
			return (400, "Parameter [piece] not specified!")
			#raise web.HTTPBadRequest(reason="Parameter [piece] not specified!")
		
		try:
			to_square = post_data["to"].lower()
		except (KeyError, ValueError):
			#raise web.HTTPBadRequest(reason="Parameter [to] not specified!")
			return (400, "Parameter [piece] not specified!")
		
		# The parameter from is either a piece or a position.
		move_str = piece + to_square
		my_legal_moves = [str(self.board.piece_at(move.from_square)).upper()
			+ chess.SQUARE_NAMES[move.to_square]
			for move in self.board.legal_moves]
		#str_my_legal_moves = str(my_legal_moves)
		str_my_legal_moves = self.legal_moves()
		#print(str_my_legal_moves)
		
		# Check if the move is a valid formatted move.
		try:
			if move_str[0] == "P":
				move = self.board.parse_san(move_str[1:])
			else:
				move = self.board.parse_san(move_str)
		except (ValueError):
			# Invalid move specified.
			#prompt = "Move: " + move_str + " is invalid!\nValid moves are: " + str(my_legal_moves)
			#raise web.HTTPBadRequest(reason=prompt)
			return (400, str_my_legal_moves)
			
		# Check if the move is legal.
		if move in self.board.legal_moves and move_str in my_legal_moves:
			self.board.push(move)
			#self.gamestate['moves'] = str(my_legal_moves)
			if (self.gamestate['turn'] == 'white'):
				self.gamestate['turn'] = 'black'
			else:
				self.gamestate['turn'] = 'white'
				
			str_my_legal_moves = self.legal_moves()
			return (202, str_my_legal_moves)
			
			
		else:
			#prompt = "Move: " + move_str + " is illegal!\nLegal moves are: " + str(my_legal_moves)
			#raise web.HTTPBadRequest(reason=prompt)
			return (400, str_my_legal_moves)
			

	def undo_move(self):
	    if len(self.board.move_stack) > 0:
	        self.board.pop()
			
			
	def legal_moves(self):
		my_legal_moves = [str(self.board.piece_at(move.from_square)).upper()
			+ chess.SQUARE_NAMES[move.to_square]
			for move in self.board.legal_moves]
		str_my_legal_moves = str(my_legal_moves)
		return str_my_legal_moves
			
	
if __name__ == "__main__":
	main()
	
	
	
	
	
	
	
