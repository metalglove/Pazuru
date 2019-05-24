package puzzle.services;

import java.util.List;
import java.util.Optional;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import puzzle.entities.Hitori;
import puzzle.repositories.HitoriRepository;
import com.google.common.collect.Lists;

@Service
public class HitoriService {

    @Autowired
    private HitoriRepository hitoriRepository;

    public List<Hitori> getAll() {
        return Lists.newArrayList(hitoriRepository.findAll());
	}

	public Optional<Hitori> find(String hitoriId) {
        return hitoriRepository.findById(Long.parseLong(hitoriId));
	}

	public Hitori save(Hitori hitoriToSave) {
		return hitoriRepository.save(hitoriToSave);
	}

}