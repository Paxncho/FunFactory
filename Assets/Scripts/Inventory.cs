using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviourSingleton<Inventory> {

    //Interface for the "Inventory" items
    public interface IItem {
        string Name  { get; }
        string Code  { get; }
        int Quantity { get; set; }

        Sprite Sprite { get; }
        Color Color   { get; }
    }

        //Attributes of the Inventory.

    public int Money { get; set; }

    //Dictionaries to store all the items that the user can have.
    Dictionary<string, IItem> materials = new Dictionary<string, IItem>();
    Dictionary<string, IItem> pieces = new Dictionary<string, IItem>();
    Dictionary<string, IItem> workers = new Dictionary<string, IItem>();
    Dictionary<string, IItem> toys = new Dictionary<string, IItem>();

        //Public methods to connect with other classes

    public void Add(IItem item, int quantity, Type type) {
        switch (type) {
            case Type.Material:
                if (materials.ContainsKey(item.Code)) {
                    materials[item.Code].Quantity += quantity;
                } else {
                    materials.Add(item.Code, item);
                }
                break;

            case Type.Piece:
                if (pieces.ContainsKey(item.Code)) {
                    pieces[item.Code].Quantity += quantity;
                } else {
                    pieces.Add(item.Code, item);
                }
                break;

            case Type.Toy:
                if (toys.ContainsKey(item.Code)) {
                    toys[item.Code].Quantity += quantity;
                } else {
                    toys.Add(item.Code, item);
                }
                break;

            case Type.Worker:
                if (workers.ContainsKey(item.Code)) {
                    workers[item.Code].Quantity += quantity;
                } else {
                    workers.Add(item.Code, item);
                }
                break;
        }
    }

    // TODO
    // void LoadInventory();
    // void SaveInventory();

    //Possible Types of Items
    public enum Type { Material, Piece, Worker, Toy }
}
