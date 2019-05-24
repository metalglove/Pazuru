package puzzle.repositories;

import org.springframework.data.repository.CrudRepository;
// import org.springframework.data.repository.PagingAndSortingRepository;
// import org.springframework.data.rest.core.annotation.RepositoryRestResource;
import org.springframework.stereotype.Repository;

import puzzle.entities.Hitori;

// @RepositoryRestResource(collectionResourceRel = "hitori", path = "hitori")
@Repository
public interface HitoriRepository extends CrudRepository<Hitori, Long> {

}