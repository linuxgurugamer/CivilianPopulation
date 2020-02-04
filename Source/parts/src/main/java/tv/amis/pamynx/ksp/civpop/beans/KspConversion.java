package tv.amis.pamynx.ksp.civpop.beans;

public class KspConversion extends KspConfig<KspConversionField>{

	public KspConversion() {
		super();
	}

	@Override
	public KspConversionField[] values() {
		return KspConversionField.values();
	}

	@Override
	public String getName() {
		return this.get(KspConversionField.PART_name);
	}

}
