package tv.amis.pamynx.ksp.civpop;

import java.io.IOException;
import java.util.Map;

import org.apache.poi.ss.usermodel.Sheet;
import org.apache.poi.ss.usermodel.Workbook;
import org.apache.poi.xssf.usermodel.XSSFWorkbook;
import org.hamcrest.Matchers;
import org.junit.Assert;
import org.junit.Test;

public class KspConfigLoaderShould {

	@Test
	public void retreive_sheet_indexes_from_PART() throws IOException {
		DataLoader loader = new DataLoader();

		try (Workbook wb = new XSSFWorkbook(this.getClass().getClassLoader().getResourceAsStream("PARTS.xlsx"))) {
			Sheet partSheet = wb.getSheet("PART");
			Map<String, Integer> cellIndexes = loader.getPartLoader().readSheetIndexes(partSheet);
			
			Assert.assertThat(cellIndexes.get("name"), Matchers.is(0));
		}

	}

	@Test
	public void retreive_sheet_indexes_from_MODEL() throws IOException {
		DataLoader loader = new DataLoader();

		try (Workbook wb = new XSSFWorkbook(this.getClass().getClassLoader().getResourceAsStream("PARTS.xlsx"))) {
			Sheet modelSheet = wb.getSheet("MODEL");
			Map<String, Integer> cellIndexes = loader.getModelLoader().readSheetIndexes(modelSheet);
			
			Assert.assertThat(cellIndexes.get("PART_name"), Matchers.is(0));
		}

	}

}
