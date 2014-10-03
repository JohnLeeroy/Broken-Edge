using UnityEngine;
using System.Collections;

public class Reload : Action {

	static Reload instance;
	
	public static Reload Instance
	{
     get 
      {
         if (instance == null)
         {
            instance = new Reload();
         }
         return instance;
      }	
	}
	
	private Reload() : base(1)
	{
		
	}
	
	public void Execute(Unit _unit)
	{
		_unit.selectedWeapon.Reload();
	}
}
