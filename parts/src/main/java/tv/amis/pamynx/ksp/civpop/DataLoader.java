package tv.amis.pamynx.ksp.civpop;

import java.io.IOException;
import java.util.Collection;
import java.util.HashMap;
import java.util.Map;

import org.apache.poi.ss.usermodel.Row;
import org.apache.poi.ss.usermodel.Sheet;
import org.apache.poi.ss.usermodel.Workbook;
import org.apache.poi.xssf.usermodel.XSSFWorkbook;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

import tv.amis.pamynx.ksp.civpop.beans.KspModel;
import tv.amis.pamynx.ksp.civpop.beans.KspModelField;
import tv.amis.pamynx.ksp.civpop.beans.KspPart;
import tv.amis.pamynx.ksp.civpop.beans.KspPartField;

public class DataLoader {

	private final Logger logger = LoggerFactory.getLogger(this.getClass());

	private KspConfigLoader<KspPartField, KspPart> partLoader;
	private KspConfigLoader<KspModelField, KspModel> modelLoader;
	
	public DataLoader() {
		super();
		partLoader = new KspConfigLoader<KspPartField, KspPart>(() -> new KspPart());
		modelLoader = new KspConfigLoader<KspModelField, KspModel>(() -> new KspModel());
	}
	
	
	public Collection<KspPart> load(String data) {
		Map<String, KspPart> parts;
		try (Workbook wb = new XSSFWorkbook(this.getClass().getClassLoader().getResourceAsStream(data))) {
			parts = partLoader.loadFrom(wb.getSheet("PART"));

			Map<String, KspModel> models = modelLoader.loadFrom(wb.getSheet("MODEL"));
			parts.values()
				.forEach(part -> part.setModel(models.get(part.getName())));
		} catch (IOException e) {
			logger.error(e.getMessage(),  e);
			throw new ConfigBuilderException(e);
		}
		return parts.values();
	}

	public KspConfigLoader<KspPartField, KspPart> getPartLoader() {
		return partLoader;
	}

	public KspConfigLoader<KspModelField, KspModel> getModelLoader() {
		return modelLoader;
	}
}
