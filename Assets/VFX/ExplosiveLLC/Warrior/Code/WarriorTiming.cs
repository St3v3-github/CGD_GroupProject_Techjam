/// <summary>
/// Contains timing for locking the Warrior’s movement and action during animation, and also timing for attack chaining windows for button presses.
/// </summary>

using UnityEngine;

namespace WarriorAnims
{
	public class WarriorTiming:MonoBehaviour
    {
		[HideInInspector] public WarriorController warriorController;

		/// <summary>
		/// Lock timing for all the Warrior attacks and actions.
		/// </summary>
		public float TimingLock(Warrior warrior, string action)
        {
			float timing = 0f;
			if (warrior == Warrior.Archer)
			{
				if (action == "attack1") timing = 0.7f;
				else if (action == "attack2") timing = 0.7f;
				else if (action == "attack3") timing = 0.7f;
				else if (action == "dash") timing = 0.6f;
				else if (action == "lighthit1") timing = 1f;
				else if (action == "move1") timing = 1f;
				else if (action == "move2") timing = 1.1f;
				else if (action == "range1") timing = 0.7f;
				else if (action == "range2") timing = 1.2f;
				else if (action == "revive") timing = 1f;
				else if (action == "sheath") timing = 1f;
				else if (action == "special1") timing = 1.4f;
			}
			else if (warrior == Warrior.Brute)
			{
				if (action == "attack1") timing = 1f;
				else if (action == "attack2") timing = 1.1f;
				else if (action == "attack3") timing = 1.4f;
				else if (action == "dash") timing = 1.4f;
				else if (action == "jumpattack") timing = 1.1f;
				else if (action == "lighthit1") timing = 1f;
				else if (action == "move1") timing = 1.7f;
				else if (action == "range1") timing = 2.3f;
				else if (action == "revive") timing = 1.6f;
				else if (action == "special1") timing = 2.1f;
			}
			else if (warrior == Warrior.Crossbow)
			{
				if (action == "attack1") timing = 0.8f;
				else if (action == "dash") timing = 0.7f;
				else if (action == "jumpattack") timing = 0.7f;
				else if (action == "range1") timing = 0.8f;
				else if (action == "range2") timing = 0.8f;
				else if (action == "move1") timing = 1f;
				else if (action == "special1") timing = 1.1f;
				else if (action == "sheath") timing = 1f;
				else if (action == "revive") timing = 1.3f;
				else if (action == "lighthit1") timing = 1f;

			}
			else if (warrior == Warrior.Hammer)
			{
				if (action == "attack1") timing = 1.35f;
				else if (action == "attack2") timing = 1.5f;
				else if (action == "attack3") timing = 1.6f;
				else if (action == "dash") timing = 1.3f;
				else if (action == "jumpattack") timing = 0.8f;
				else if (action == "lighthit1") timing = 1f;
				else if (action == "move1") timing = 2.6f;
				else if (action == "range1") timing = 1.8f;
				else if (action == "revive") timing = 1.4f;
				else if (action == "sheath") timing = 1.2f;
				else if (action == "special1") timing = 1.5f;
			}
			else if (warrior == Warrior.Karate)
			{
				if (action == "attack1") timing = 0.7f;
				else if (action == "attack2") timing = 0.8f;
				else if (action == "attack3") timing = 0.8f;
				else if (action == "attack4") timing = 0.5f;
				else if (action == "attack5") timing = 1.2f;
				else if (action == "attack6") timing = 0.7f;
				else if (action == "attack7") timing = 1.1f;
				else if (action == "attack8") timing = 0.8f;
				else if (action == "attack9") timing = 0.8f;
				else if (action == "dash") timing = 0.9f;
				else if (action == "jumpattack") timing = 0.8f;
				else if (action == "lighthit1") timing = 0.6f;
				else if (action == "move1") timing = 1.1f;
				else if (action == "move2") timing = 1.1f;
				else if (action == "range1") timing = 1.1f;
				else if (action == "range2") timing = 1.1f;
				else if (action == "revive") timing = 0.9f;
				else if (action == "special1") timing = 1.7f;
				else if (action == "special2") timing = 1.4f;
			}
			else if (warrior == Warrior.Knight)
			{
				if (action == "attack1") timing = 0.6f;
				else if (action == "attack2") timing = 0.7f;
				else if (action == "attack3") timing = 0.9f;
				else if (action == "dash") timing = 1.1f;
				else if (action == "dash2") timing = 0.65f;
				else if (action == "lighthit1") timing = 0.75f;
				else if (action == "move1") timing = 1f;
				else if (action == "move2") timing = 1.1f;
				else if (action == "range1") timing = 1.3f;
				else if (action == "revive") timing = 1.7f;
				else if (action == "sheath") timing = 1f;
				else if (action == "special1") timing = 1.3f;
				else if (action == "special2") timing = 0.9f;
			}
			else if (warrior == Warrior.Mage)
			{
				if (action == "attack1") timing = 1.1f;
				else if (action == "attack2") timing = 1.3f;
				else if (action == "attack3") timing = 0.9f;
				else if (action == "dash") timing = 1.1f;
				else if (action == "jumpattack") timing = 0.85f;
				else if (action == "lighthit1") timing = 0.8f;
				else if (action == "move1") timing = 1.5f;
				else if (action == "range1") timing = 1.8f;
				else if (action == "range2") timing = 1.9f;
				else if (action == "revive") timing = 1.3f;
				else if (action == "sheath") timing = 0.8f;
				else if (action == "special1") timing = 1.9f;
				else if (action == "special2") timing = 1.9f;
			}
			else if (warrior == Warrior.Ninja)
			{
				if (action == "attack1") timing = 0.6f;
				else if (action == "attack2") timing = 0.8f;
				else if (action == "attack3") timing = 1.4f;
				else if (action == "dash") timing = 0.7f;
				else if (action == "lighthit1") timing = 0.5f;
				else if (action == "move1") timing = 1f;
				else if (action == "move2") timing = 1.1f;
				else if (action == "range1") timing = 1f;
				else if (action == "range2") timing = 0.9f;
				else if (action == "range3") timing = 0.95f;
				else if (action == "revive") timing = 0.8f;
				else if (action == "sheath") timing = 1.1f;
				else if (action == "special1") timing = 1.7f;
				else if (action == "special2") timing = 1;
			}
			else if (warrior == Warrior.Sorceress)
			{
				if (action == "attack1") timing = 1.1f;
				else if (action == "attack2") timing = 1.2f;
				else if (action == "attack3") timing = 1.15f;
				else if (action == "attack4") timing = 0.7f;
				else if (action == "attack5") timing = 1.1f;
				else if (action == "attack6") timing = 1.3f;
				else if (action == "attack7") timing = 1.3f;
				else if (action == "attack8") timing = 1.3f;
				else if (action == "dash") timing = 0.8f;
				else if (action == "lighthit1") timing = 1f;
				else if (action == "move1") timing = 1.2f;
				else if (action == "move2") timing = 1.2f;
				else if (action == "move3") timing = 1f;
				else if (action == "range1") timing = 1.4f;
				else if (action == "revive") timing = 0.8f;
				else if (action == "special1") timing = 1.6f;
				else if (action == "special2") timing = 2.3f;
				else if (action == "special3") timing = 0.9f;
			}
			else if (warrior == Warrior.Spearman)
			{
				if (action == "attack1") timing = 0.9f;
				else if (action == "attack2") timing = 0.95f;
				else if (action == "attack3") timing = 1.3f;
				else if (action == "attack4") timing = 1f;
				else if (action == "attack5") timing = 1f;
				else if (action == "dash") timing = 0.7f;
				else if (action == "jumpattack") timing = 0.7f;
				else if (action == "lighthit1") timing = 0.8f;
				else if (action == "move1") timing = 0.95f;
				else if (action == "range1") timing = 1.2f;
				else if (action == "revive") timing = 1.15f;
				else if (action == "sheath") timing = 0.8f;
				else if (action == "special1") timing = 1f;
			}
			else if (warrior == Warrior.Swordsman)
			{
				if (action == "attack1") timing = 0.9f;
				else if (action == "attack2") timing = 1.1f;
				else if (action == "attack3") timing = 1f;
				else if (action == "dash") timing = 0.7f;
				else if (action == "jumpattack") timing = 0.8f;
				else if (action == "lighthit1") timing = 0.9f;
				else if (action == "move1") timing = 1f;
				else if (action == "range1") timing = 0.9f;
				else if (action == "revive") timing = 1f;
				else if (action == "sheath") timing = 0.75f;
				else if (action == "special1") timing = 1.1f;
			}
			else if (warrior == Warrior.TwoHanded)
			{
				if (action == "attack1") timing = 1.1f;
				else if (action == "attack2") timing = 1f;
				else if (action == "attack3") timing = 1.3f;
				else if (action == "jumpattack") timing = 0.8f;
				else if (action == "range1") timing = 2.5f;
				else if (action == "move1") timing = 1.4f;
				else if (action == "special1") timing = 1.45f;
				else if (action == "dash") timing = 0.95f;
				else if (action == "sheath") timing = 1f;
				else if (action == "revive") timing = 1.1f;
				else if (action == "lighthit1") timing = 1.1f;
			}
			return timing;
        }

		/// <summary>
		/// Chain timing windows for the Warrior attack chain button presses.
		/// </summary>
		public float TimingChain(Warrior warrior, string action)
		{
			float timing = 0f;
			if (warrior == Warrior.Brute)
			{
				if (action == "attack1") timing = 0.4f;
				else if (action == "attack1end") timing = 0.9f;
				else if (action == "attack2") timing = 0.4f;
				else if (action == "attack2end") timing = 0.7f;
			}
			else if (warrior == Warrior.Crossbow)
			{
				if (action == "attack1") timing = 0.8f;
				else if (action == "attack1end") timing = 0.8f;
				else if (action == "attack2") timing = 0.9f;
				else if (action == "attack2end") timing = 0.9f;
			}
			else if(warrior == Warrior.Hammer)
			{
				if(action == "attack1") timing = 0.6f;
				else if(action == "attack1end") timing = 1.2f;
				else if(action == "attack2") timing = 0.6f;
				else if(action == "attack2end") timing = 1.2f;
			}
			else if(warrior == Warrior.Karate)
			{
				if (action == "attack1") timing = 0.4f;
				else if (action == "attack1end") timing = 0.8f;
				else if (action == "attack2") timing = 0.3f;
				else if (action == "attack2end") timing = 0.6f;
			}
			else if (warrior == Warrior.Knight)
			{
				if (action == "attack1") timing = 0.1f;
				else if (action == "attack1end") timing = 0.8f;
				else if (action == "attack2") timing = 0.3f;
				else if (action == "attack2end") timing = 0.9f;
			}
			else if (warrior == Warrior.Mage)
			{
				if (action == "attack1") timing = 0.4f;
				else if (action == "attack1end") timing = 1.2f;
				else if (action == "attack2") timing = 0.4f;
				else if (action == "attack2end") timing = 1.2f;
			}
			else if (warrior == Warrior.Ninja)
			{
				if (action == "attack1") timing = 1.2f;
				else if (action == "attack1end") timing = 1.2f;
				else if (action == "attack2") timing = 0.2f;
				else if (action == "attack2end") timing = 0.8f;
			}
			else if (warrior == Warrior.Sorceress)
			{
				if (action == "attack1") timing = 0.3f;
				else if (action == "attack1end") timing = 1.4f;
				else if (action == "attack2") timing = 1.2f;
				else if (action == "attack2end") timing = 1.2f;
			}
			else if (warrior == Warrior.Spearman)
			{
				if (action == "attack1") timing = 0.2f;
				else if (action == "attack1end") timing = 0.8f;
				else if (action == "attack2") timing = 0.6f;
				else if (action == "attack2end") timing = 1.1f;
			}
			else if (warrior == Warrior.Swordsman)
			{
				if (action == "attack1") timing = 0.6f;
				else if (action == "attack1end") timing = 0.6f;
				else if (action == "attack2") timing = 1.1f;
				else if (action == "attack2end") timing = 1.1f;
			}
			else if (warrior == Warrior.TwoHanded)
			{
				if (action == "attack1") timing = 0.6f;
				else if (action == "attack1end") timing = 1f;
				else if (action == "attack2") timing = 0.5f;
				else if (action == "attack2end") timing = 0.8f;
			}
			return timing;
		}
	}
}