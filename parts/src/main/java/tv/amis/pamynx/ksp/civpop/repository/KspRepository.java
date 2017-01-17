package tv.amis.pamynx.ksp.civpop.repository;

import java.util.Collection;
import java.util.List;
import java.util.Optional;
import java.util.stream.Collectors;

import org.apache.poi.ss.usermodel.Workbook;

import tv.amis.pamynx.ksp.civpop.beans.KspConversion;
import tv.amis.pamynx.ksp.civpop.beans.KspConversionField;
import tv.amis.pamynx.ksp.civpop.beans.KspInternal;
import tv.amis.pamynx.ksp.civpop.beans.KspInternalField;
import tv.amis.pamynx.ksp.civpop.beans.KspModel;
import tv.amis.pamynx.ksp.civpop.beans.KspModelField;
import tv.amis.pamynx.ksp.civpop.beans.KspModule;
import tv.amis.pamynx.ksp.civpop.beans.KspModuleField;
import tv.amis.pamynx.ksp.civpop.beans.KspPart;
import tv.amis.pamynx.ksp.civpop.beans.KspPartField;
import tv.amis.pamynx.ksp.civpop.beans.KspResource;
import tv.amis.pamynx.ksp.civpop.beans.KspResourceField;

public class KspRepository {

	private KspConfigRepository<KspPart>       parts;
	private KspConfigRepository<KspModel>      models;
	private KspConfigRepository<KspInternal>   internals;
	private KspConfigRepository<KspResource>   resources;
	private KspConfigRepository<KspModule>     modules;
	private KspConfigRepository<KspConversion> conversions;

	public KspRepository() {
		this.parts       = new KspConfigRepository<KspPart>();
		this.models      = new KspConfigRepository<KspModel>();
		this.internals   = new KspConfigRepository<KspInternal>();
		this.resources   = new KspConfigRepository<KspResource>();
		this.modules     = new KspConfigRepository<KspModule>();
		this.conversions = new KspConfigRepository<KspConversion>();
	}
	
	public void load(Workbook wb) {
		KspConfigLoader<KspPartField, KspPart> partLoader = new KspConfigLoader<>(() -> new KspPart());
		partLoader.loadListFrom(wb.getSheet("PART")).stream().forEach(config -> parts.add(config));

		KspConfigLoader<KspModelField, KspModel> modelLoader = new KspConfigLoader<>(() -> new KspModel());
		modelLoader.loadListFrom(wb.getSheet("MODEL")).stream().forEach(config -> models.add(config));

		KspConfigLoader<KspInternalField, KspInternal> internalLoader = new KspConfigLoader<>(() -> new KspInternal());
		internalLoader.loadListFrom(wb.getSheet("INTERNAL")).stream().forEach(config -> internals.add(config));

		KspConfigLoader<KspResourceField, KspResource> resourceLoader = new KspConfigLoader<>(() -> new KspResource());
		resourceLoader.loadListFrom(wb.getSheet("RESOURCE")).stream().forEach(config -> resources.add(config));

		KspConfigLoader<KspModuleField, KspModule> moduleLoader = new KspConfigLoader<>(() -> new KspModule());
		moduleLoader.loadListFrom(wb.getSheet("MODULE")).stream().forEach(config -> modules.add(config));

		KspConfigLoader<KspConversionField, KspConversion> conversionLoader = new KspConfigLoader<>(() -> new KspConversion());
		conversionLoader.loadListFrom(wb.getSheet("CONVERSION")).stream().forEach(config -> conversions.add(config));

	}

	public Collection<KspPart> getParts() {
		return parts.getList();
	}

	public Collection<KspConversion> getConversions() {
		return conversions.getList();
	}

	public KspModel getModel(KspPart part) {
		return this.models.getList().stream()
				.filter(model -> model.get(KspModelField.PART_name).equals(part.get(KspPartField.name)))
				.findAny()
				.orElse(null);
	}

	public KspInternal getInternal(KspPart part) {
		return this.internals.getList().stream()
				.filter(internal -> internal.get(KspInternalField.PART_name).equals(part.get(KspPartField.name)))
				.findAny()
				.orElse(null);
	}

	public List<KspResource> getResources(KspPart part) {
		return this.resources.getList().stream()
				.filter(resource -> resource.get(KspResourceField.PART_name).equals(part.get(KspPartField.name)))
				.collect(Collectors.toList());
	}

	public List<KspModule> getModules(KspPart part) {
		return this.modules.getList().stream()
				.filter(resource -> resource.get(KspModuleField.PART_name).equals(part.get(KspPartField.name)))
				.collect(Collectors.toList());
	}

	public List<KspConversion> getConversions(KspModule module) {
		String name = Optional
				.ofNullable(module.get(KspModuleField.ConverterName))
				.orElse(module.get(KspModuleField.name));
		
		return this.conversions.getList().stream()
				.filter(conversion -> conversion.get(KspConversionField.PART_name).equals(module.get(KspModuleField.PART_name)))
				.filter(conversion -> conversion.get(KspConversionField.ConverterName).equals(name))
				.collect(Collectors.toList());
	}

}
