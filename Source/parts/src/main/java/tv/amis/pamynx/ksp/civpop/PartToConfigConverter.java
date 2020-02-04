package tv.amis.pamynx.ksp.civpop;

import static tv.amis.pamynx.ksp.civpop.ConfigBuilder.LINE_SEPARATOR;

import java.io.ByteArrayInputStream;
import java.io.IOException;
import java.io.InputStream;
import java.net.URISyntaxException;
import java.nio.charset.StandardCharsets;
import java.nio.file.Files;
import java.nio.file.Paths;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;
import java.util.stream.Collectors;

import tv.amis.pamynx.ksp.civpop.beans.KspConversion;
import tv.amis.pamynx.ksp.civpop.beans.KspConversionField;
import tv.amis.pamynx.ksp.civpop.beans.KspInternalField;
import tv.amis.pamynx.ksp.civpop.beans.KspModelField;
import tv.amis.pamynx.ksp.civpop.beans.KspModule;
import tv.amis.pamynx.ksp.civpop.beans.KspModuleField;
import tv.amis.pamynx.ksp.civpop.beans.KspPart;
import tv.amis.pamynx.ksp.civpop.beans.KspPartField;
import tv.amis.pamynx.ksp.civpop.beans.KspResource;
import tv.amis.pamynx.ksp.civpop.beans.KspResourceField;
import tv.amis.pamynx.ksp.civpop.beans.KspResourceType;
import tv.amis.pamynx.ksp.civpop.repository.KspRepository;

public class PartToConfigConverter {

	private KspRepository repository;
	
	public PartToConfigConverter(KspRepository repository) {
		super();
		this.repository = repository;
	}

	public InputStream toConfig(KspPart part) throws IOException, URISyntaxException {
		String config = new String(Files.readAllBytes(Paths.get(getClass().getClassLoader().getResource("part-template.cfg").toURI())));

		for (KspPartField f : KspPartField.values()) {
			if (!f.isOption()){
				config = config.replaceAll("%"+f.name()+"%", part.get(f));
			} else {
				if (part.get(f) != null) {
					config = config.replaceAll("%"+f.name()+"%", f.name()+" = "+part.get(f));
				} else {
					config = config.replaceAll("    %"+f.name()+"%"+LINE_SEPARATOR, "");
				}
			}
		}
		config = config.replaceAll("%MODEL%", getBlockModel(part));
		config = config.replaceAll("%INTERNAL%"+LINE_SEPARATOR, getBlockInternal(part));
		config = config.replaceAll("%RESOURCE%"+LINE_SEPARATOR, getBlockResource(part));
		config = config.replaceAll("%MODULE%"+LINE_SEPARATOR, getBlockModule(part));
		config = config.replaceAll("%node_stack%", getBlockNodeStack(part));

		//config = config.replaceAll("%explosionPotential%", "explosionPotential = "+part.get(KspPartField.explosionPotential));
		return new ByteArrayInputStream(config.getBytes(StandardCharsets.UTF_8));
	}

	private String getBlockModel(KspPart part) {
		List<String> res = new ArrayList<>();
		if (repository.getModel(part) != null) {
			res.add("    MODEL");
			res.add("    {");
			res.add("        model = "+repository.getModel(part).get(KspModelField.model));
			if (repository.getModel(part).get(KspModelField.scale) != null) {
				res.add("        scale = "+repository.getModel(part).get(KspModelField.scale));
			}
			if (repository.getModel(part).get(KspModelField.texture) != null) {
				Arrays.stream(repository.getModel(part).get(KspModelField.texture).split("\n"))
					.forEach(s -> res.add("        texture = "+s));
			}
			res.add("    }");
		}
		return res.stream().collect(Collectors.joining("\n"));
	}

	private String getBlockInternal(KspPart part) {
		List<String> res = new ArrayList<>();
		if (repository.getInternal(part) != null) {
			res.add("    INTERNAL");
			res.add("    {");
			res.add("        name = "+repository.getInternal(part).get(KspInternalField.name));
			res.add("    }");
			res.add("");
		}
		return res.stream().collect(Collectors.joining("\n"));
	}

	private String getBlockResource(KspPart part) {
		List<String> res = new ArrayList<>();
		if (repository.getResources(part) != null) {
			for (KspResource resource : repository.getResources(part)) {
				KspResourceType type = KspResourceType.valueOf(resource.get(KspResourceField.name));
				if (type != null) {
					res.add("    RESOURCE");
					res.add("    {");
					res.add("        name = "+resource.get(KspResourceField.name));
					if (resource.get(KspResourceField.amount) != null) {
						res.add("        amount = "+resource.get(KspResourceField.amount));
					}
					if (resource.get(KspResourceField.maxAmount) != null) {
						res.add("        maxAmount = "+resource.get(KspResourceField.maxAmount));
					}
					if (resource.get(KspResourceField.isTweakable) != null) {
						res.add("        isTweakable = "+resource.get(KspResourceField.isTweakable));
					}
					res.add("    }");
				}
			}
			res.add("");
		}
		return res.stream().collect(Collectors.joining("\n"));
	}
	

	private String getBlockModule(KspPart part) {
		List<String> res = new ArrayList<>();
		if (repository.getModules(part) != null) {
			for (KspModule module : repository.getModules(part)) {
				res.add("    MODULE");
				res.add("    {");
				for (KspModuleField f : KspModuleField.values()) {
					if (f != KspModuleField.PART_name) {
						String value = module.get(f);
						if (value != null) {
							res.add("        "+f.name()+" = "+value);
						}
					}
				}
				if ("ModuleResourceConverter".equals(module.get(KspModuleField.name))) {
					for (KspConversion conversion : repository.getConversions(module)) {
						res.add("        "+conversion.get(KspConversionField.TYPE)+"_RESOURCE");
						res.add("        {");
						for (KspConversionField f : KspConversionField.values()) {
							if (f != KspConversionField.PART_name 
							 && f != KspConversionField.TYPE 
							 && f != KspConversionField.ConverterName
							 ) {
								String value = conversion.get(f);
								if (value != null) {
									res.add("            "+f.name()+" = "+value);
								}
							}
						}
						res.add("        }");
					}
				}
				if ("ModuleScienceLab".equals(module.get(KspModuleField.name))) {
					for (KspConversion conversion : repository.getConversions(module)) {
						res.add("        RESOURCE_"+conversion.get(KspConversionField.TYPE));
						res.add("        {");
						for (KspConversionField f : KspConversionField.values()) {
							if (f != KspConversionField.PART_name 
							 && f != KspConversionField.TYPE 
							 && f != KspConversionField.ConverterName
							 ) {
								String value = conversion.get(f);
								if (value != null) {
									res.add("            "+f.name()+" = "+value);
								}
							}
						}
						res.add("        }");
					}
				}
				if ("ModuleResourceHarvester".equals(module.get(KspModuleField.name))) {
					for (KspConversion conversion : repository.getConversions(module)) {
						res.add("        "+conversion.get(KspConversionField.TYPE)+"_RESOURCE");
						res.add("        {");
						for (KspConversionField f : KspConversionField.values()) {
							if (f != KspConversionField.PART_name 
							 && f != KspConversionField.TYPE 
							 && f != KspConversionField.ConverterName
							 ) {
								String value = conversion.get(f);
								if (value != null) {
									res.add("            "+f.name()+" = "+value);
								}
							}
						}
						res.add("        }");
					}
				}
				res.add("    }");
			}
			res.add("");
		}
		return res.stream().collect(Collectors.joining("\n"));
	}


	private String getBlockNodeStack(KspPart part) {
		List<String> res = new ArrayList<>();
		for (KspPartField f : KspPartField.values()) {
			if (f.name().startsWith("node_stack") && part.get(f) != null) {
				Arrays.stream(part.get(f).split("\n"))
					.forEach(line -> res.add("    "+f.name()+" = "+line));
			}
		}
		return res.stream().collect(Collectors.joining("\n"));
	}
}
