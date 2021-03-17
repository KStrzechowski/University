//using System;
using System;

namespace Lab05_pl
{
    abstract class Shape2D
    {
        protected static int Count;
        protected int ObjectNumber;
        public Shape2D()
        {
            Count++;
            ObjectNumber = Count;
            Console.WriteLine("Shape2D ({0}) created", ObjectNumber);
        }

        public abstract double CalculateArea();
        public virtual string PrintShape2D()
        {
            string print = "Shape(" + ObjectNumber + ")";
            return print;
        }
        ~Shape2D()
        {
            Console.WriteLine("Shape2D ({0}) destroyed", ObjectNumber);
        }
    }

    class Circle: Shape2D
    {
        double value;
        public Circle(double value): base()
        {
            
            this.value = value;
            Console.WriteLine("Circle({0}) with radius = {1} created", ObjectNumber, value);
        }
        public override double CalculateArea()
        {
            return Math.PI * value * value;
        }
        public override string PrintShape2D()
        {
            string print = "Circle(r=" + value + ")";
            return print;
        }
        ~Circle()
        {
            Console.WriteLine("Circle ({0}) destroyed", ObjectNumber);
        }

    }

    abstract class Shape3D
    {
        public static int NumberOfCreatedObjects;
        protected Shape2D baseShape;
        protected double height;
        public Shape3D(Shape2D baseShape, double height)
        {
            NumberOfCreatedObjects++;
            this.baseShape = baseShape;
            this.height = height;
        }
        public abstract double CalculateCapacity();
        public virtual string PrintShape3D()
        {
            string result = "Shape3D with base "+ baseShape.PrintShape2D() +" and height: " + height;
            return result;
        }
    }

    class Cone: Shape3D
    {
        public static new int NumberOfCreatedObjects;
        public Cone(Shape2D baseShape, double height): base(baseShape, height) { NumberOfCreatedObjects++; }
        public override double CalculateCapacity()
        {
            return (baseShape.CalculateArea() * height) / 3;
        }
        public override string PrintShape3D()
        {
            base.PrintShape3D();
            string result = "Cone(h=" + height + ") with base: " + baseShape.PrintShape2D();
            return result;
        }
    }

    class Cylinder : Shape3D
    {
        public static new int NumberOfCreatedObjects;
        public Cylinder(Shape2D baseShape, double height) : base(baseShape, height) { NumberOfCreatedObjects++; }
        public override double CalculateCapacity()
        {
            return (baseShape.CalculateArea() * height);
        }
        public new string PrintShape3D()
        {
            base.PrintShape3D();
            string result = "Cylinder(h=" + height + ") with base: " + baseShape.PrintShape2D();
            return result;
        }
    }


}
