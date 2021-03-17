using System;
using System.Collections.Generic;

namespace Lab12
{

public enum CollectionOperation
    {
    Add,
    Remove,
    ValueChanged
    }

public interface IObservableCollection
    {
    event EventHandler<CollectionChangedEventArgs> CollectionChanged;
    string Name { get; }
    void Add(object value);
    void Remove(object value);
    }

public interface IChangeNotifing
    {
    event EventHandler NameChanged;
    }

public class CollectionChangedEventArgs : EventArgs
    {

    public CollectionChangedEventArgs(CollectionOperation operation, object value)
        {
        this.Operation = operation;
        this.Value = value;
        }

    public CollectionOperation Operation { get; private set; }
    public object Value { get; private set; }

    }

public class ObservableCollection : IObservableCollection
    {
        private List<IObservableCollection> list = new List<IObservableCollection>();
        public string Name { get; }
        public ObservableCollection(string name)
        {
            Name = name;
        }
        public event EventHandler<CollectionChangedEventArgs> CollectionChanged;
        public void Add(object value)
        {
            CollectionChanged?.Invoke(this, new CollectionChangedEventArgs(CollectionOperation.Add, value));
            list.Add(value as IObservableCollection);
            if (value is IChangeNotifing)
            {
                ((IChangeNotifing)value).NameChanged += ObservableCollection_NameChanged;
            }
        }
        public void Remove(object value)
        {
            CollectionChanged?.Invoke(this, new CollectionChangedEventArgs(CollectionOperation.Remove, value));
            list.Remove(value as IObservableCollection);
            if (value is IChangeNotifing)
            {
                ((IChangeNotifing)value).NameChanged -= ObservableCollection_NameChanged;
            }
        }
        private void ObservableCollection_NameChanged(object sender, EventArgs e)
        {
            Console.WriteLine($"Collection {this.Name} changed, value: {sender} was changed");
        }
    }

public class SimpleWatcher
    {
        public void Watch(IObservableCollection obj)
        {
            obj.CollectionChanged += Obj_CollectionChanged;
        }
        public void StopWatching(IObservableCollection obj)
        {
            obj.CollectionChanged -= Obj_CollectionChanged;
        }
        private void Obj_CollectionChanged(object sender, CollectionChangedEventArgs e)
        {
            Console.WriteLine("Collection changed");
        }
    }
public class SmartWatcher
    {
        public void Watch(IObservableCollection obj)
        {
            obj.CollectionChanged += Obj_CollectionChanged;
        }
        public void StopWatching(IObservableCollection obj)
        {
            obj.CollectionChanged -= Obj_CollectionChanged;
        }
        private void Obj_CollectionChanged(object sender, CollectionChangedEventArgs e)
        {
            Console.Write($"collection: {(sender as IObservableCollection).Name} changed, value: {e.Value} was ");
            if(e.Operation.ToString() == "Add") { Console.WriteLine("added"); }
            if (e.Operation.ToString() == "Remove") { Console.WriteLine("removed"); }
        }
    }

public class NotifingObject : IChangeNotifing
    {
        private string _Name;
        public string Name 
        {
            get { return _Name; }
            set
            {
                NameChanged?.Invoke(value, new EventArgs());
                _Name = value;
            }

        }
        public NotifingObject() { }
        public event EventHandler NameChanged;

        public override string ToString()
        {
            return _Name;
        }

    }

public class Program
    {

    public static void Main()
        {
        // ETAP 1
        Console.WriteLine("\nETAP 1\n");

        var collection = new ObservableCollection("[collection 1]");
        var simpleWatcher = new SimpleWatcher();

        collection.Add("[First item]");

        simpleWatcher.Watch(collection);

        collection.Add("[Second item]");
        collection.Remove("[First item]");

        // ETAP 2
        Console.WriteLine("\nETAP 2\n");

        var smartWatcher = new SmartWatcher();
        smartWatcher.Watch(collection);
        collection.Add("[Third item]");
        Console.WriteLine("-------------------------------");

        simpleWatcher.StopWatching(collection);
        collection.Remove("[Third item]");

        // ETAP 3
        Console.WriteLine("\nETAP 3\n");

        var object1 = new NotifingObject();
        var object2 = new NotifingObject();
        object1.Name = "[o1]";
        object2.Name = "[o2]";

        collection.Add(object1);
        collection.Add(object2);

        Console.WriteLine("-------------------------------");

        object1.Name = "[new o1]";
        object2.Name = "[new o2]";

        Console.WriteLine("-------------------------------");

        collection.Remove(object2);

        Console.WriteLine("-------------------------------");

        object1.Name = "[even newer o1]";
        object2.Name = "[even newer o2]";

        Console.WriteLine();
        }

    }

}
