# for creation of the database.

CREATE DATABASE pazuru DEFAULT CHARACTER SET utf8 COLLATE utf8_unicode_ci;
CREATE USER 'enigmatologist'@'localhost' IDENTIFIED BY 'unsolvable';
GRANT ALL ON pazuru.* TO 'enigmatologist'@'localhost' IDENTIFIED BY 'unsolvable';

# for storing a puzzle
# request
curl -i -X POST -H "Content-Type:application/json" -d '{ "puzzleType": "Sudoku", "originalPuzzle" : "034007008080065000000300070200000700710040096005000001050002000000170060600900430",  "solvedPuzzle" : "534297618187465329962381574246819753718543296395726841459632187823174965671958432" }' http://localhost:8090/puzzles/savePuzzle

# response
