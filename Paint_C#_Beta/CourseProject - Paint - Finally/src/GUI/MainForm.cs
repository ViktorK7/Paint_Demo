using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Draw
{
	public partial class MainForm : Form
	{
		private DialogProcessor dialogProcessor = new DialogProcessor();
		public Point currentPoint = new Point();
		public Point oldPoint = new Point();
		public Pen pen = new Pen(Color.Black, 5);
		public Graphics graphics;
		public bool isDrawing = false;

		public MainForm()
		{
			InitializeComponent();
			graphics = drawPanel.CreateGraphics();
			pen.SetLineCap(LineCap.Round, LineCap.Round, DashCap.Round);
		}

		void ExitToolStripMenuItemClick(object sender, EventArgs e)
		{
			Close();
		}
		
		void ViewPortPaint(object sender, PaintEventArgs e)
		{
			dialogProcessor.ReDraw(sender, e);
		}
		
		void DrawRectangleSpeedButtonClick(object sender, EventArgs e)
		{
			dialogProcessor.AddRandomRectangle();
			
			statusBar.Items[0].Text = "Последно действие: Рисуване на правоъгълник";
			
			viewPort.Invalidate();
		}

		private void DrawEllipseButton_Click(object sender, EventArgs e)
		{
			dialogProcessor.AddRandomEllipse();

			statusBar.Items[0].Text = "Последно действие: Рисуване на елипса";

			viewPort.Invalidate();
		}

		private void DrawTrinangleSpeedButton_Click(object sender, EventArgs e)
		{
			dialogProcessor.AddRandomTriangle();

			statusBar.Items[0].Text = "Последно действие: Рисуване на триъгълник";

			viewPort.Invalidate();
		}

		void ViewPortMouseDown(object sender, MouseEventArgs e)
		{
			if(e.Button == MouseButtons.Right)
			{
				if (pickUpSpeedButton.CheckOnClick)
				{
					var sel = dialogProcessor.ContainsPoint(e.Location);

					if (sel != null)
					{
						if (dialogProcessor.Selection.Contains(sel))
						{
							dialogProcessor.Selection.Remove(sel);
						}
						else
						{
							dialogProcessor.Selection.Add(sel);
						}
						statusBar.Items[0].Text = "Последно действие: Селекция на примитив";
						viewPort.Invalidate();
					}
				}
			}
			else if(e.Button == MouseButtons.Left)
			{
				dialogProcessor.IsDragging = true;
				dialogProcessor.LastLocation = e.Location;
			}
		}

		void ViewPortMouseMove(object sender, MouseEventArgs e)
		{
			if (dialogProcessor.IsDragging)
			{
				if (dialogProcessor.Selection != null) statusBar.Items[0].Text = "Последно действие: Влачене";
				dialogProcessor.TranslateTo(e.Location);
				viewPort.Invalidate();
			}
		}

		void ViewPortMouseUp(object sender, MouseEventArgs e)
		{
			dialogProcessor.IsDragging = false;
		}

		private void ItemBackgroundColorButton_Click(object sender, EventArgs e)
		{
			if(colorDialog1.ShowDialog() == DialogResult.OK)
			{
				dialogProcessor.SetFillColor(colorDialog1.Color);
				statusBar.Items[0].Text = "Последно действие: Смяна на цвета на фона";
				viewPort.Invalidate();
			}
		}

		private void SetBorderColorButton_Click(object sender, EventArgs e)
		{
			if(colorDialog1.ShowDialog() == DialogResult.OK)
			{
				dialogProcessor.SetBorderColor(colorDialog1.Color);
				statusBar.Items[0].Text = "Последно действие: Промяна в цвета на границата";
				viewPort.Invalidate();
				pen.Color = colorDialog1.Color;
			}
		}

		private void GroupButton_Click(object sender, EventArgs e)
		{
			dialogProcessor.GroupSelected();
			statusBar.Items[0].Text = "Последно дейстие: Групиране";
			viewPort.Invalidate();
		}

		private void UngroupButton_Click(object sender, EventArgs e)
		{
			dialogProcessor.UnGroup();
			statusBar.Items[0].Text = "Последно действие: Разгрупиране";
			viewPort.Invalidate();
		}

		private void RotateRightButton_Click(object sender, EventArgs e)
		{
			dialogProcessor.Rotate(30);
			statusBar.Items[0].Text = "Последно действие: Завъртане на дясно";
			viewPort.Invalidate();
		}

		private void RotateLeftButton_Click(object sender, EventArgs e)
		{
			dialogProcessor.Rotate(-30);
			statusBar.Items[0].Text = "Последно действие: Завъртане на ляво";
			viewPort.Invalidate();
		}

		private void OpacityTrackbar_Scroll(object sender, EventArgs e)
		{
			dialogProcessor.SetOpacity(OpacityTrackbar.Value);
			statusBar.Items[0].Text = "Последно действие: Добавяне на прозрачност";
			viewPort.Invalidate();
		}

		private void InsertStringButton_Click(object sender, EventArgs e)
		{
			if (!string.IsNullOrEmpty(textBoxString.Text))
			{
				dialogProcessor.AddRandomString(textBoxString.Text);
				statusBar.Items[0].Text = "Последно действие: Рисуване на тектс";
				textBoxString.Clear();
				viewPort.Invalidate();
			}
		}

		private void DeleteButton_Click(object sender, EventArgs e)
		{
			dialogProcessor.DeleteSelected();
			statusBar.Items[0].Text = "Последно действие: Изтриване на селектираните примитиви";
			viewPort.Invalidate();
			drawPanel.Controls.Clear();
			drawPanel.Invalidate();
		}

		private void newFileToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (dialogProcessor.ShapeList.Count > 0)
			{
				if (MessageBox.Show("Искате ли да запаметите промените?", "Запаметяване", MessageBoxButtons.YesNo) == DialogResult.Yes)
				{
					if (saveFileDialog1.ShowDialog() == DialogResult.OK)
					{
						dialogProcessor.MySerialize(dialogProcessor.ShapeList, saveFileDialog1.FileName);
						statusBar.Items[0].Text = "Последно действие: Изчистване на работното място";
					}
				}
				dialogProcessor.ShapeList.Clear();
				viewPort.Invalidate();
				drawPanel.Controls.Clear();
				drawPanel.Invalidate();
			}
		}

		private void openFileToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (openFileDialog1.ShowDialog() == DialogResult.OK)
			{
				dialogProcessor.ShapeList = (List<Shape>)dialogProcessor.MyDeSerialize(openFileDialog1.FileName);
				statusBar.Items[0].Text = "Последно действие: Отваряне";
				viewPort.Invalidate();
			}
		}

		private void saveFileToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (saveFileDialog1.ShowDialog() == DialogResult.OK)
			{
				dialogProcessor.MySerialize(dialogProcessor.ShapeList, saveFileDialog1.FileName);
				statusBar.Items[0].Text = "Последно действие: Запазване";
				drawPanel.Controls.Clear();
				drawPanel.Invalidate();
			}
		}

		private void groupToolStripMenuItem_Click(object sender, EventArgs e)
		{
			dialogProcessor.GroupSelected();
			statusBar.Items[0].Text = "Последно действие: Групиране";
			viewPort.Invalidate();
		}

		private void unGroupToolStripMenuItem_Click(object sender, EventArgs e)
		{
			dialogProcessor.UnGroup();
			statusBar.Items[0].Text = "Последно действие: Разгрупиране";
			viewPort.Invalidate();
		}

		private void borderColorToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (colorDialog1.ShowDialog() == DialogResult.OK)
			{
				dialogProcessor.SetBorderColor(colorDialog1.Color);
				statusBar.Items[0].Text = "Последно действие: Смяна на цвета на границата";
				viewPort.Invalidate();
			}
		}

		private void fillColorToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (colorDialog1.ShowDialog() == DialogResult.OK)
			{
				dialogProcessor.SetFillColor(colorDialog1.Color);
				statusBar.Items[0].Text = "Последно действие: Смяна на цвета";
				viewPort.Invalidate();
			}
		}

		private void rectangleToolStripMenuItem_Click(object sender, EventArgs e)
		{
			dialogProcessor.AddRandomRectangle();

			statusBar.Items[0].Text = "Последно действие: Рисуване на правоъгълник";

			viewPort.Invalidate();
		}

		private void elipseToolStripMenuItem_Click(object sender, EventArgs e)
		{
			dialogProcessor.AddRandomEllipse();

			statusBar.Items[0].Text = "Последно действие: Рисуване на елипса";

			viewPort.Invalidate();
		}

		private void triangleToolStripMenuItem_Click(object sender, EventArgs e)
		{
			dialogProcessor.AddRandomTriangle();

			statusBar.Items[0].Text = "Последно действие: Рисуване на триъгълник";

			viewPort.Invalidate();
		}

		private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
		{
			MessageBox.Show("Nikolay Nedyalkov 1801321081 \n        Software engineering", "Information For Project", MessageBoxButtons.OK);
		}
	
		public void drawPanel_MouseDown(object sender, MouseEventArgs e)
		{
			oldPoint = e.Location;
		}

		private void drawPanel_MouseMove(object sender, MouseEventArgs e)
		{
			if (isDrawing)
			{
				if (e.Button == MouseButtons.Left)
				{
					currentPoint = e.Location;
					graphics.DrawLine(pen, oldPoint, currentPoint);
					oldPoint = currentPoint;
				}
			}
		}

		private void PenButton_Click(object sender, EventArgs e)
		{
			if (isDrawing == false)
			{
				isDrawing = true;
				statusBar.Items[0].Text = "Последно действие: Рисуване с молив (Отключено)";
			}
			else
			{
				isDrawing = false;
				statusBar.Items[0].Text = "Последно действие: Рисуване с молив (Заключено)";
			}
		}

		private void toolStripButton1_Click(object sender, EventArgs e)
		{
			dialogProcessor.AddRandomCircleLine();
			viewPort.Invalidate();
		}
	}
}
