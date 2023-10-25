using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;

namespace SchoolGradesMean.Windows;
public class FormMain : Form {
    Button?[] gradeButtons = { null, null, null, null, null };
    Button? gradeReset = null;
    Button? gradeUndo = null;
    Label? grades = null;
    Label? mean = null;
    List<decimal> grade = new();

    public FormMain() {
        Text = "Mean grade calculator";
        Size = new Size(285, 460);
        StartPosition = FormStartPosition.CenterScreen;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        Init();
    }

    public void Init() {
        for (int i = 1; i<=5; i++) {
            gradeButtons[i-1] = new Button {
                Text = i.ToString(),
                Cursor = Cursors.Hand,
                Size = new Size(50, 50),
                Location = new Point(50*i-40, 10),
            };
            gradeButtons[i - 1]!.Click += GradeButtonClickListener;
        }
        Controls.AddRange(gradeButtons!);

        gradeUndo = new Button {
            Text = "Undo latest",
            Cursor = Cursors.Hand,
            Size = new Size(120, 50),
            Location = new Point(10, 65),
        };
        gradeUndo.Click += GradeUndoClickListener;
        Controls.Add(gradeUndo);

        gradeReset = new Button {
            Text = "Reset",
            Cursor = Cursors.Hand,
            Size = new Size(120, 50),
            Location = new Point(140, 65),
        };
        gradeReset.Click += GradeResetClickListener;
        Controls.Add(gradeReset);

        grades = new Label {
            AutoSize = true,
            MaximumSize = new Size(250, 200),
            Location = new Point(10, 140),
            Size = new Size(250, 200),
            Font = new Font(Font.FontFamily, 22f),
            TextAlign = ContentAlignment.TopCenter
        };
        Controls.Add(grades);

        mean = new Label {
            Text = "0,00",
            AutoSize = true,
            MaximumSize = new Size(250, 200),
            Location = new Point(100, 360),
            Size = new Size(250, 200),
            Font = new Font(Font.FontFamily, 22f),
            TextAlign = ContentAlignment.TopCenter
        };
        Controls.Add(mean);

        Controls.Add(new Label {
            Location = new Point(0, 340),
            Size = new Size(400, 4),
            AutoSize = false,
            Text = "",
            BorderStyle = BorderStyle.Fixed3D,
            Height = 2,
        });

        KeyPress += KeyPressListener;
    }

    private void KeyPressListener(object? sender, KeyPressEventArgs e) {
        var idx = e.KeyChar - '0' - 1;
        MessageBox.Show(idx.ToString());
        if (idx is >= 0 and <= 4) {
            gradeButtons[idx]?.PerformClick();
        } else if (e.KeyChar == '\b') {
            gradeUndo?.PerformClick();
        } else if (e.KeyChar == (char)Keys.Delete) {
            gradeReset?.PerformClick();
        }
    }

    void GradeResetClickListener(object? sender, EventArgs e) {
        if (grade.Count > 0) {
            grade.Clear();
            UpdateGradesLabels();
        }
    }

    void GradeUndoClickListener(object? sender, EventArgs e) {
        if (grade.Count > 0) {
            grade.RemoveAt(grade.Count - 1);
            UpdateGradesLabels();
        }
    }

    void GradeButtonClickListener(object? sender, EventArgs e) {
        if (sender is Button button) {
            var num = ushort.Parse(button.Text);
            grade.Add(num);
            UpdateGradesLabels();
        }
    }

    void UpdateGradesLabels() {
        if (grades != null)
            grades.Text = string.Join(", ", grade);
        CalculateMean();
    }

    void CalculateMean() {
        if (mean != null) {
            decimal gradeSum;
            if ((gradeSum = grade.Sum()) != 0)
                gradeSum = Math.Round(grade.Sum() / grade.Count, 2);
            mean.Text = gradeSum.ToString();
        }
    }
}
