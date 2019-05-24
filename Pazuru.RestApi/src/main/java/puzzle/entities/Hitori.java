package puzzle.entities;

import javax.persistence.Entity;
import javax.persistence.GeneratedValue;
import javax.persistence.GenerationType;
import javax.persistence.Id;

import org.springframework.hateoas.ResourceSupport;

@Entity
public class Hitori extends ResourceSupport {

	@Id
	@GeneratedValue(strategy = GenerationType.AUTO)
	private Long id;
	private String solvedHitori;
	private String originalHitori;

	public String getSolvedHitori() {
		return solvedHitori;
	}

	public String getOriginalHitori() {
		return originalHitori;
	}

	public void setOriginalHitori(String originalHitori) {
		this.originalHitori = originalHitori;
	}

	public void setSolvedHitori(String solvedHitori) {
		this.solvedHitori = solvedHitori;
    }
    
    public Long getHitoriId() {
        return id;
    }

    public void setHitoriId(Long id) {
        this.id = id;
    }
}
