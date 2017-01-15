package tv.amis.pamynx.ksp.civpop.beans;

public class KspModule extends KspConfig<KspModuleField>{

	public KspModule() {
		super();
	}

	@Override
	public KspModuleField[] values() {
		return KspModuleField.values();
	}

	@Override
	public String getName() {
		return this.get(KspModuleField.PART_name);
	}
}
