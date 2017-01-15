package tv.amis.pamynx.ksp.civpop.repository;

import java.util.ArrayList;
import java.util.List;

import tv.amis.pamynx.ksp.civpop.beans.KspConfig;

public class KspConfigRepository<T extends KspConfig<?>> {

	private List<T> data;
	
	public KspConfigRepository() {
		super();
		this.data = new ArrayList<>();
	}

	public void add(T part) {
		this.data.add(part);
	}
	
	public List<T> getList() {
		return data;
	}

}
