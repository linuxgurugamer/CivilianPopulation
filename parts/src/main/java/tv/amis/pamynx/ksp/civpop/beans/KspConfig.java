package tv.amis.pamynx.ksp.civpop.beans;

import java.util.HashMap;
import java.util.List;
import java.util.Map;

import org.apache.poi.ss.usermodel.Cell;
import org.apache.poi.ss.usermodel.CellType;
import org.apache.poi.ss.usermodel.Row;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

import tv.amis.pamynx.ksp.civpop.ConfigBuilderException;

public abstract class KspConfig<F extends KspConfigField> {

	private Map<F, String> configMap;

	public KspConfig() {
		super();
		this.configMap = new HashMap<>();
	}

	public String get(F f) {
		return configMap.get(f);
	}

	public String set(F f, String value) {
		return configMap.put(f, value);
	}
	
	public abstract F[] values();
	
	public abstract String getName();

}
