package puzzle;

import org.springframework.data.repository.PagingAndSortingRepository;
import org.springframework.data.rest.core.annotation.RepositoryRestResource;

@RepositoryRestResource(collectionResourceRel = "sudoku", path = "sudoku")
public interface SudokuRepository extends PagingAndSortingRepository<Sudoku, Long> {

}