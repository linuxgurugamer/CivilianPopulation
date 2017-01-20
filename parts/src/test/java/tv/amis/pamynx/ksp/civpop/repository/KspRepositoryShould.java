package tv.amis.pamynx.ksp.civpop.repository;

import java.io.IOException;

import org.apache.poi.ss.usermodel.Workbook;
import org.apache.poi.xssf.usermodel.XSSFWorkbook;
import org.hamcrest.Matchers;
import org.junit.Assert;
import org.junit.Test;

public class KspRepositoryShould {

	@Test
	public void retreive_parts() throws IOException {
		KspRepository repository = new KspRepository();

		try (Workbook wb = new XSSFWorkbook(this.getClass().getClassLoader().getResourceAsStream("PARTS.xlsx"))){
			repository.load(wb);
		};

		Assert.assertThat(repository.getParts().size(), Matchers.is(37));
	}

	@Test
	public void retreive_conversions() throws IOException {
		KspRepository repository = new KspRepository();

		try (Workbook wb = new XSSFWorkbook(this.getClass().getClassLoader().getResourceAsStream("PARTS.xlsx"))){
			repository.load(wb);
		};

		Assert.assertThat(repository.getConversions().size(), Matchers.is(23));
	}

}
