# for creation of the database.

CREATE DATABASE pazuru DEFAULT CHARACTER SET utf8 COLLATE utf8_unicode_ci; 
CREATE USER 'enigmatologist'@'localhost' IDENTIFIED BY 'unsolvable'; 
GRANT ALL ON pazuru.* TO 'enigmatologist'@'localhost' IDENTIFIED BY 'unsolvable';

# for storing a puzzle
# request
curl -i -X POST -H "Content-Type:application/json" -d '{  "originalSudoku" : "034007008080065000000300070200
000700710040096005000001050002000000170060600900430",  "solvedSudoku" : "5342976181874653299623815742468197537
18543296395726841459632187823174965671958432" }' http://localhost:8090/sudoku

# response
HTTP/1.1 201
Location: http://localhost:8090/sudoku/1
Content-Type: application/hal+json;charset=UTF-8
Transfer-Encoding: chunked
Date: Thu, 23 May 2019 22:42:24 GMT

{
  "solvedSudoku" : "534297618187465329962381574246819753718543296395726841459632187823174965671958432",
  "originalSudoku" : "034007008080065000000300070200000700710040096005000001050002000000170060600900430",
  "_links" : {
    "self" : {
      "href" : "http://localhost:8090/sudoku/1"
    },
    "sudoku" : {
      "href" : "http://localhost:8090/sudoku/1"
    }
  }
}