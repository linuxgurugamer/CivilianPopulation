package tv.amis.pamynx.ksp.civpop;

import java.io.IOException;
import java.util.Collection;
import java.util.Map;

import org.apache.poi.ss.usermodel.Sheet;
import org.apache.poi.ss.usermodel.Workbook;
import org.apache.poi.xssf.usermodel.XSSFWorkbook;
import org.hamcrest.Matchers;
import org.junit.Assert;
import org.junit.Test;

import tv.amis.pamynx.ksp.civpop.beans.KspPart;

public class DataLoaderShould {

	@Test
	public void retreive_parts_from_data() {
		DataLoader loader = new DataLoader();
		
		Collection<KspPart> parts = loader.load("PARTS.xlsx");
		
		Assert.assertThat(parts.size(), Matchers.is(1));
	}
	
}
