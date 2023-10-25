using System;
using System.Collections.Generic;
using System.Linq;
using Modern.Forms;
using System.Drawing;
using ContentAlignment = Modern.Forms.ContentAlignment;

namespace SchoolGradesMean.Modern;
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
        Init();
    }

    public void Init() {
        for (int i = 1; i<=5; i++) {
            gradeButtons[i-1] = new Button {
                Text = i.ToString(),
                Cursor = Cursors.Hand,
                Size = new Size(50, 50),
                Location = new Point(50*i-40, 40),
            };
            gradeButtons[i - 1]!.Click += GradeButtonClickListener;
        }
        Controls.AddRange(gradeButtons!);

        gradeUndo = new Button {
            Text = "Undo latest",
            Cursor = Cursors.Hand,
            Size = new Size(120, 50),
            Location = new Point(10, 105),
        };
        gradeUndo.Click += GradeUndoClickListener;
        Controls.Add(gradeUndo);

        gradeReset = new Button {
            Text = "Reset",
            Cursor = Cursors.Hand,
            Size = new Size(120, 50),
            Location = new Point(140, 105),
        };
        gradeReset.Click += GradeResetClickListener;
        Controls.Add(gradeReset);

        grades = new Label {
            AutoSize = true,
            MaximumSize = new Size(250, 200),
            Location = new Point(10, 180),
            Size = new Size(250, 200),
            TextAlign = ContentAlignment.TopCenter
        };
        Controls.Add(grades);

        mean = new Label {
            Text = "0,00",
            AutoSize = true,
            MaximumSize = new Size(250, 200),
            Location = new Point(100, 400),
            Size = new Size(250, 200),
            TextAlign = ContentAlignment.TopCenter
        };
        Controls.Add(mean);

        Controls.Add(new Label {
            Location = new Point(0, 380),
            Size = new Size(400, 2),
            AutoSize = false,
            Text = "",
            Height = 2,
        });
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
