using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class Player : LivingCreature
    {
        public int Gold { get; set; }
        public int ExperiencePoints { get; set; }
        public int Level { get; set; }
        public Location CurrentLocation { get; set; }
        public List<InventoryItem> Inventory { get; set; }
        public List<PlayerQuest> Quests { get; set; }

        public Player(int currentHitPoints, int maximumHitPoints, int gold, int experiencePoints, int level) : base(currentHitPoints, maximumHitPoints)
        {
            Gold = gold;
            ExperiencePoints = experiencePoints;
            Level = level;

            Inventory = new List<InventoryItem>();
            Quests = new List<PlayerQuest>();
        }
        public bool HasRequiredItemToEnterThisLocation(Location location)
        {
            if (location.ItemRequiredToEnter == null)
            {
                //nem kell item bemenni
                return true;
            }

            //megvan-e a szukseges item
            foreach (InventoryItem ii in Inventory)
            {
                if (ii.Details.ID == location.ItemRequiredToEnter.ID)
                {
                    //megtalaltuk az itemet
                    return true;
                }
            }
            //nem talaltuk meg az itemet
            return false;
        }
        public bool HasThisQuest(Quest quest)
        {
            foreach (PlayerQuest playerQuest in Quests)
            {
                if (playerQuest.Details.ID == quest.ID)
                {
                    return true;
                }
            }
            return false;
        }
        public bool CompletedThisQuest(Quest quest)
        {
            foreach (PlayerQuest playerQuest in Quests)
            {
                if (playerQuest.Details.ID == quest.ID)
                {
                    return playerQuest.IsCompleted;
                }
            }
            return false;
        }
        public bool HasAllQuestCompletionItems(Quest quest)
        {
            //megvan-e minden item a quest befejezesere
            foreach (QuestCompletionItem qci in quest.QuestCompletionItems)
            {
                bool foundItemInPlayersInventory = false;

                //megnezzuk hogy megvan-e a szukseges item valamint elegendo szamban
                foreach (InventoryItem ii in Inventory)
                {
                    if (ii.Details.ID == qci.Details.ID) //megtalaltuk az itemet
                    {
                        foundItemInPlayersInventory = true;
                        if (ii.Quantity < qci.Quantity) //nincs eleg item
                        {
                            return false;
                        }
                    }
                }
                //nincsenek az itemek az inventoryban
                if (!foundItemInPlayersInventory)
                {
                    return false;
                }
            }
            // megvannak az itemek es eleg beloluk
            return true;
        }
        public void RemoveQuestCompletionItems(Quest quest)
        {
            foreach (QuestCompletionItem qci in quest.QuestCompletionItems)
            {
                foreach (InventoryItem ii in Inventory)
                {
                    if (ii.Details.ID == qci.Details.ID)
                    {
                        //elvesszuk a szukseges itemet hogy befelyezhessuk a questet
                        ii.Quantity -= qci.Quantity;
                        break;
                    }
                }
            }
        }
        public void AddItemToInventory(Item itemToAdd)
        {
            foreach (InventoryItem ii in Inventory)
            {
                if (ii.Details.ID == itemToAdd.ID)
                {
                    //van ilyen itemjuk, hozzaadunk 1-et
                    ii.Quantity++;
                    return;//hozzaadtuk az itemet, kiszallunk
                }
            }
            //nincs ilyen item, adunk 1 ilyet
            Inventory.Add(new InventoryItem(itemToAdd, 1));
        }
        public void MarkQuestCompleted(Quest quest)
        {
            //megkeressuk a questet a questek kozott
            foreach(PlayerQuest pq in Quests)
            {
                if(pq.Details.ID == quest.ID)
                {
                    //teljesitettnek jeloljuk
                    pq.IsCompleted = true;
                    return; //megvan a quest, teljesitettnek jeloltuk,kiszallunk
                }
            }
        }
    }
}
