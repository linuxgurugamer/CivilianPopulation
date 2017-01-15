package tv.amis.pamynx.ksp.civpop.beans;

public class KspResource extends KspConfig<KspResourceField>{

	public KspResource() {
		super();
	}

	@Override
	public KspResourceField[] values() {
		return KspResourceField.values();
	}

	@Override
	public String getName() {
		return this.get(KspResourceField.PART_name);
	}

}
