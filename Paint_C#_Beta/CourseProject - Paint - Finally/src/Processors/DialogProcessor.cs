using Draw.src.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

namespace Draw
{
	public class DialogProcessor : DisplayProcessor
	{
		#region Constructor
		
		public DialogProcessor()
		{
		}
		
		#endregion
		
		#region Properties
		
		/// <summary>
		/// Избран елемент.
		/// </summary>
		private List<Shape> selection = new List<Shape>();
		public List<Shape> Selection {
			get { return selection; }
			set { selection = value; }
		}
		
		/// <summary>
		/// Дали в момента диалога е в състояние на "влачене" на избрания елемент.
		/// </summary>
		private bool isDragging;
		public bool IsDragging {
			get { return isDragging; }
			set { isDragging = value; }
		}
		
		/// <summary>
		/// Последна позиция на мишката при "влачене".
		/// Използва се за определяне на вектора на транслация.
		/// </summary>
		private PointF lastLocation;
		public PointF LastLocation {
			get { return lastLocation; }
			set { lastLocation = value; }
		}
		
		#endregion
		
		
		public void AddRandomRectangle()
		{
			Random rnd = new Random();
			int x = rnd.Next(600,1300);
			int y = rnd.Next(100,400);
			
			RectangleShape rect = new RectangleShape(new Rectangle(x,y,100,200));
			rect.FillColor = Color.YellowGreen;
			rect.BorderColor = Color.Blue;
			rect.Opacity = 255;

			ShapeList.Add(rect);
		}
		
		public Shape ContainsPoint(PointF point)
		{
			for(int i = ShapeList.Count - 1; i >= 0; i--){
				if (ShapeList[i].Contains(point)){
					return ShapeList[i];
				}	
			}
			return null;
		}
		
		public void TranslateTo(PointF p)
		{
			foreach (var item in Selection)
			{
				item.Move(p.X - lastLocation.X, p.Y - lastLocation.Y);
			}
			lastLocation = p;
		}
		
		public void AddRandomEllipse()
		{
			Random random = new Random();
			int x = random.Next(600, 1300);
			int y = random.Next(100, 400);

			EllipseShape ellipse = new EllipseShape(new Rectangle(x, y, 200, 100));
			ellipse.FillColor = Color.YellowGreen;
			ellipse.BorderColor = Color.Blue;
			ellipse.Opacity = 255;

			ShapeList.Add(ellipse);
		}

		public void AddRandomTriangle()
		{
			Random random = new Random();
			int x = random.Next(600, 1300);
			int y = random.Next(100, 400);

			TriangleShape triangle = new TriangleShape(new Rectangle(x, y, 100, 200));
			triangle.FillColor = Color.YellowGreen;
			triangle.BorderColor = Color.Blue;
			triangle.Opacity = 255;

			ShapeList.Add(triangle);
		}

		public void AddRandomString(string text)
		{
			Random rnd = new Random();
			int x = rnd.Next(600, 1300);
			int y = rnd.Next(100, 400);

			SizeF stringSize = new SizeF();
			stringSize = TextRenderer.MeasureText(text, new Font("Arial", 16));

			StringShape stringShape = new StringShape(new Rectangle(x, y, (int)stringSize.Width, (int)stringSize.Height));
			stringShape.Text = text;
			stringShape.FillColor = Color.Black;
			stringShape.Opacity = 255;

			ShapeList.Add(stringShape);
		}

		public override void Draw(Graphics grfx)
		{
			base.Draw(grfx);
			foreach (var item in Selection)
			{
				item.RotateShape(grfx);
				grfx.DrawRectangle(Pens.Black, item.Rectangle.Left - 3, item.Rectangle.Top - 3, item.Rectangle.Width + 6, item.Rectangle.Height + 6);
				grfx.ResetTransform();
			}

		}

		public void SetFillColor(Color color)
		{
			foreach(var item in Selection)
			{
				item.FillColor = color;
			}
		}

		public void SetBorderColor(Color color)
		{
			foreach(var item in Selection)
			{
				item.BorderColor = color;
			}
		}

		public void Rotate(float angle)
		{
			if (Selection.Count != 0)
			{
				foreach (var item in Selection)
				{
					int currentAngle = (int)item.Angle;
					item.GroupRotate(currentAngle + angle);
					item.Angle = currentAngle + angle;
				}
			}
		}
		public void SetOpacity(int opacity)
		{

			foreach (var item in Selection)
			{
				item.GroupOpacity(opacity);
				item.Opacity = opacity;

			}
		}

		public void GroupSelected()
		{
			if (Selection.Count < 2) return;

			float minX = float.PositiveInfinity;
			float minY = float.PositiveInfinity;
			float maxX = float.NegativeInfinity;
			float maxY = float.NegativeInfinity;
			foreach (var item in Selection)
			{
				if (minX > item.Location.X)
				{
					minX = item.Location.X;
				}
				if (minY > item.Location.Y)
				{
					minY = item.Location.Y;
				}
				if (maxX < item.Location.X + item.Width)
				{
					maxX = item.Location.X + item.Width;
				}
				if (maxY < item.Location.Y + item.Height)
				{
					maxY = item.Location.Y + item.Height;
				}
			}
			var group = new GroupShape(new RectangleF(minX, minY, maxX - minX, maxY - minY));
			group.SubItems = Selection;
			foreach (var item in Selection)
			{
				ShapeList.Remove(item);
			}
			Selection = new List<Shape>();
			Selection.Add(group);
			ShapeList.Add(group);
		}

		public void UnGroup()
		{
			for (int i = 0; i < Selection.Count; i++)
			{
				if (Selection[i].GetType().Equals(typeof(GroupShape)))
				{
					GroupShape group = (GroupShape)Selection[i];
					ShapeList.AddRange(group.SubItems);
					group.SubItems.Clear();
					ShapeList.Remove(group);
					Selection[i] = null;
					group = null;
					ShapeList.Remove(Selection[i]);
					Selection.Remove(Selection[i]);
				}
			}
			Selection = new List<Shape>();
		}

		public void MySerialize(object obj, string filePath = null)
		{
			IFormatter formatter = new BinaryFormatter();
			Stream stream;
			if (filePath != null)
			{
				stream = new FileStream(filePath + ".drw", FileMode.Create);
			}
			else
			{
				stream = new FileStream("MyFile.drw", FileMode.Create, FileAccess.Write, FileShare.None);
			}
			formatter.Serialize(stream, obj);
			stream.Close();
		}

		public object MyDeSerialize(string filePath = null)
		{
			object obj;
			IFormatter formatter = new BinaryFormatter();
			Stream stream;
			if (filePath != null)
			{
				stream = new FileStream(filePath,
									 FileMode.Open,
									 FileAccess.Read, FileShare.None);
			}
			else
			{
				stream = new FileStream("MyFile.drw",
									FileMode.Open);
			}
			obj = formatter.Deserialize(stream);
			stream.Close();
			return obj;
		}

		public void DeleteSelected()
		{
			foreach (var item in Selection)
			{
				ShapeList.Remove(item);
			}
			Selection.Clear();
		}

		public void AddRandomCircleLine()
		{
			Random rnd = new Random();
			int x = rnd.Next(600, 1300);
			int y = rnd.Next(100, 400);
			ExamShape exam = new ExamShape(new Rectangle(x, y, 200, 100));
			exam.BorderColor = Color.Black;
			ShapeList.Add(exam);


		}
	}
}
