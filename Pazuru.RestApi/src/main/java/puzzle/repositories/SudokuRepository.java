package puzzle.repositories;

import java.util.Optional;

import org.springframework.data.repository.CrudRepository;
// import org.springframework.data.repository.PagingAndSortingRepository;
// import org.springframework.data.rest.core.annotation.RepositoryRestResource;
import org.springframework.stereotype.Repository;

import puzzle.entities.Sudoku;

// @RepositoryRestResource(collectionResourceRel = "sudoku", path = "sudoku")
@Repository
public interface SudokuRepository extends CrudRepository<Sudoku, Long> {
    Optional<Sudoku> findBySolvedSudoku(String solvedSudoku);
}