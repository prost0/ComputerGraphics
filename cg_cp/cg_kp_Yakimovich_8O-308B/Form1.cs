//Yakimovich Alexander 8O-308B
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;


public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();

        dataPointCount = 0;
        withMarkers = checkBoxOfMerkersEnabled.Checked;
        panelOfApproximation.Enabled = checkBoxOfMerkersEnabled.Checked;
        panelOfDataPoints.Enabled = checkBoxOfMerkersEnabled.Checked;

        dataPoints1 = new MyPoint[4];
        MyPoint point11 = new MyPoint(0, 0, 0, 1); // точки дефолтные 
        MyPoint point12 = new MyPoint(1, -1, 0, 1);
        MyPoint point14 = new MyPoint(3, -1, 0, 1);

        dataPoints1[0] = point11;
        dataPoints1[1] = point12;
        dataPoints1[2] = point14;

        dataPoints2 = new MyPoint[4];
        MyPoint point21 = new MyPoint(0, 0, 1, 1);
        MyPoint point22 = new MyPoint(1, -1, 1, 1);
        MyPoint point24 = new MyPoint(3, -1, 1, 1);

        dataPoints2[0] = point21;
        dataPoints2[1] = point22;
        dataPoints2[2] = point24;

        dataPoints3 = new MyPoint[4];
        MyPoint point31 = new MyPoint(0, 0, 1, 1);
        MyPoint point32 = new MyPoint(1, 1, 1, 1);
        MyPoint point34 = new MyPoint(0, 0, 0, 1);

        dataPoints3[0] = point31;
        dataPoints3[1] = point32;
        dataPoints3[2] = point34;

        dataPoints4 = new MyPoint[4];
        MyPoint point41 = new MyPoint(3, -1, 1, 1);
        MyPoint point42 = new MyPoint(3, 1, 1, 1);
        MyPoint point44 = new MyPoint(3, -1, 0, 1);

        dataPoints4[0] = point41;
        dataPoints4[1] = point42;
        dataPoints4[2] = point44;

        this.calcCurves();

        mx = 0;
        my = 0;
        cx = 0;
        cy = 0;

        scale = 100;
        mashtabK = 0;

        isMouseDown = false;
    }

    private void calcCurves()
    {
        bezierCurve1 = new BezierCurve(dataPoints1, int.Parse(textBoxOfNumberOfDrawPoints.Text));
        bezierCurve2 = new BezierCurve(dataPoints2, int.Parse(textBoxOfNumberOfDrawPoints.Text));
        bezierCurve3 = new BezierCurve(dataPoints3, int.Parse(textBoxOfNumberOfDrawPoints.Text));
        bezierCurve4 = new BezierCurve(dataPoints4, int.Parse(textBoxOfNumberOfDrawPoints.Text));

        kuntzSurface = new KunsSurface(bezierCurve1, bezierCurve2, bezierCurve3, bezierCurve4);
    }

    private void mashtabMinusButton_Click(object sender, EventArgs e)
    {
        mashtabK--;

        this.Refresh();
    }

    private void mashtabPlusButton_Click(object sender, EventArgs e)
    {
        mashtabK++;

        this.Refresh();
    }

    private void Form1_Paint(object sender, PaintEventArgs e)
    {
        e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
        double zoom_level = (scale + mashtabK) / 1000;
        double coeff = Math.Max(e.ClipRectangle.Width, e.ClipRectangle.Height) * zoom_level;

        ShiftMatrix sh = new ShiftMatrix(e.ClipRectangle.Width / 2, e.ClipRectangle.Height / 2, 0);
        ScalingMatrix sc = new ScalingMatrix(coeff, coeff, coeff);
        RotationMatrix rtx = new RotationMatrix('X', my * Math.PI / 180.0);
        RotationMatrix rty = new RotationMatrix('Y', -mx * Math.PI / 180.0);
        Matrix preobr = sh * rtx * rty * sc;

        if(isKunsCheckBox.Checked) kuntzSurface.Draw(preobr, e.Graphics, dataPointCount, withMarkers);
       // else linearSurface.Draw(preobr, e.Graphics, dataPointCount, withMarkers);

    }

    private void Form1_MouseMove(object sender, MouseEventArgs e)
    {
        if (isMouseDown)
        {
            int delta_x = e.X - cx;
            int delta_y = e.Y - cy;
            mx += delta_x;
            my += delta_y;
            cx = e.X;
            cy = e.Y;

            this.Refresh();
        }
    }

    private void Form1_MouseDown(object sender, MouseEventArgs e)
    {
        isMouseDown = true;
        cx = e.X;
        cy = e.Y;
    }

    private void Form1_MouseUp(object sender, MouseEventArgs e)
    {
        isMouseDown = false;
    }

    private void Form1_SizeChanged(object sender, EventArgs e)
    {
        this.Refresh();
    }

    private void buttonOfApply1_Click(object sender, EventArgs e)
    {
        this.calcCurves();
        this.Refresh();
    }

    private void buttonRight_Click(object sender, EventArgs e)
    {
        if (dataPointCount < 3) dataPoints1[dataPointCount].x += 1;
        if (dataPointCount > 2 && dataPointCount < 6) dataPoints2[dataPointCount - 3].x += 1;
        if (dataPointCount > 5 && dataPointCount < 9) dataPoints3[dataPointCount - 6].x += 1;
        if (dataPointCount > 8 && dataPointCount < 12) dataPoints4[dataPointCount - 9].x += 1;

        if (dataPointCount == 0) dataPoints3[dataPointCount + 2].x += 1;
        if (dataPointCount == 2) dataPoints4[dataPointCount].x += 1;
        if (dataPointCount == 5) dataPoints4[dataPointCount - 5].x += 1;
        if (dataPointCount == 3) dataPoints3[dataPointCount - 3].x += 1;

        if (dataPointCount == 8) dataPoints1[dataPointCount - 8].x += 1;
        if (dataPointCount == 6) dataPoints2[dataPointCount - 6].x += 1;
        if (dataPointCount == 9) dataPoints2[dataPointCount - 7].x += 1;

        if (dataPointCount == 11) dataPoints1[dataPointCount - 9].x += 1;
        
        calcCurves();

        this.Refresh();
    }

    private void buttonLeft_Click(object sender, EventArgs e)
    {
        if (dataPointCount < 3) dataPoints1[dataPointCount].x -= 1;
        if (dataPointCount > 2 && dataPointCount < 6) dataPoints2[dataPointCount - 3].x -= 1;
        if (dataPointCount > 5 && dataPointCount < 9) dataPoints3[dataPointCount - 6].x -= 1;
        if (dataPointCount > 8 && dataPointCount < 12) dataPoints4[dataPointCount - 9].x -= 1;

        if (dataPointCount == 0) dataPoints3[dataPointCount + 2].x -= 1;
        if (dataPointCount == 2) dataPoints4[dataPointCount].x -= 1;
        if (dataPointCount == 5) dataPoints4[dataPointCount - 5].x -= 1;
        if (dataPointCount == 3) dataPoints3[dataPointCount - 3].x -= 1;

        if (dataPointCount == 8) dataPoints1[dataPointCount - 8].x -= 1;
        if (dataPointCount == 6) dataPoints2[dataPointCount - 6].x -= 1;
        if (dataPointCount == 9) dataPoints2[dataPointCount - 7].x -= 1;

        if (dataPointCount == 11) dataPoints1[dataPointCount - 9].x -= 1;
        
        calcCurves();

        this.Refresh();
    }

    private void buttonNaNas_Click(object sender, EventArgs e)
    {
        if (dataPointCount < 3) dataPoints1[dataPointCount].z -= 1;
        if (dataPointCount > 2 && dataPointCount < 6) dataPoints2[dataPointCount - 3].z -= 1;
        if (dataPointCount > 5 && dataPointCount < 9) dataPoints3[dataPointCount - 6].z -= 1;
        if (dataPointCount > 8 && dataPointCount < 12) dataPoints4[dataPointCount - 9].z -= 1;

        if (dataPointCount == 0) dataPoints3[dataPointCount + 2].z -= 1;
        if (dataPointCount == 2) dataPoints4[dataPointCount].z -= 1;
        if (dataPointCount == 5) dataPoints4[dataPointCount - 5].z -= 1;
        if (dataPointCount == 3) dataPoints3[dataPointCount - 3].z -= 1;

        if (dataPointCount == 8) dataPoints1[dataPointCount - 8].z -= 1;
        if (dataPointCount == 6) dataPoints2[dataPointCount - 6].z -= 1;
        if (dataPointCount == 9) dataPoints2[dataPointCount - 7].z -= 1;

        if (dataPointCount == 11) dataPoints1[dataPointCount - 9].z -= 1;

        calcCurves();

        this.Refresh();
    }

    private void buttonOtNas_Click(object sender, EventArgs e)
    {
        if (dataPointCount < 3) dataPoints1[dataPointCount].z += 1;
        if (dataPointCount > 2 && dataPointCount < 6) dataPoints2[dataPointCount - 3].z += 1;
        if (dataPointCount > 5 && dataPointCount < 9) dataPoints3[dataPointCount - 6].z += 1;
        if (dataPointCount > 8 && dataPointCount < 12) dataPoints4[dataPointCount - 9].z += 1;

        if (dataPointCount == 0) dataPoints3[dataPointCount + 2].z += 1;
        if (dataPointCount == 2) dataPoints4[dataPointCount].z += 1;
        if (dataPointCount == 5) dataPoints4[dataPointCount - 5].z += 1;
        if (dataPointCount == 3) dataPoints3[dataPointCount - 3].z += 1;

        if (dataPointCount == 8) dataPoints1[dataPointCount - 8].z += 1;
        if (dataPointCount == 6) dataPoints2[dataPointCount - 6].z += 1;
        if (dataPointCount == 9) dataPoints2[dataPointCount - 7].z += 1;

        if (dataPointCount == 11) dataPoints1[dataPointCount - 9].z += 1;

        calcCurves();

        this.Refresh();
    }
    private void buttonUp_Click(object sender, EventArgs e)
    {
        if (dataPointCount < 3) dataPoints1[dataPointCount].y -= 1;
        if (dataPointCount > 2 && dataPointCount < 6) dataPoints2[dataPointCount - 3].y -= 1;
        if (dataPointCount > 5 && dataPointCount < 9) dataPoints3[dataPointCount - 6].y -= 1;
        if (dataPointCount > 8 && dataPointCount < 12) dataPoints4[dataPointCount - 9].y -= 1;

        if (dataPointCount == 0) dataPoints3[dataPointCount + 2].y -= 1;
        if (dataPointCount == 2) dataPoints4[dataPointCount].y -= 1;
        if (dataPointCount == 5) dataPoints4[dataPointCount - 5].y -= 1;
        if (dataPointCount == 3) dataPoints3[dataPointCount - 3].y -= 1;

        if (dataPointCount == 8) dataPoints1[dataPointCount - 8].y -= 1;
        if (dataPointCount == 6) dataPoints2[dataPointCount - 6].y -= 1;
        if (dataPointCount == 9) dataPoints2[dataPointCount - 7].y -= 1;

        if (dataPointCount == 11) dataPoints1[dataPointCount - 9].y -= 1;

        calcCurves();

        this.Refresh();
    }

    private void buttonDown_Click(object sender, EventArgs e)
    {
        if (dataPointCount < 3) dataPoints1[dataPointCount].y += 1;
        if (dataPointCount > 2 && dataPointCount < 6) dataPoints2[dataPointCount - 3].y += 1;
        if (dataPointCount > 5 && dataPointCount < 9) dataPoints3[dataPointCount - 6].y += 1;
        if (dataPointCount > 8 && dataPointCount < 12) dataPoints4[dataPointCount - 9].y += 1;

        if (dataPointCount == 0) dataPoints3[dataPointCount + 2].y += 1;
        if (dataPointCount == 2) dataPoints4[dataPointCount].y += 1;
        if (dataPointCount == 5) dataPoints4[dataPointCount - 5].y += 1;
        if (dataPointCount == 3) dataPoints3[dataPointCount - 3].y += 1;

        if (dataPointCount == 8) dataPoints1[dataPointCount - 8].y += 1;
        if (dataPointCount == 6) dataPoints2[dataPointCount - 6].y += 1;
        if (dataPointCount == 9) dataPoints2[dataPointCount - 7].y += 1;

        if (dataPointCount == 11) dataPoints1[dataPointCount - 9].y += 1;

        calcCurves();

        this.Refresh();
    }

    // текущие координаты курсора и координаты его предыдущего положения

    int mx, my, cx, cy;

    // масштаб

    float scale;
    double mashtabK;

    MyPoint[] dataPoints1;
    MyPoint[] dataPoints2;
    MyPoint[] dataPoints3;
    MyPoint[] dataPoints4;
    int dataPointCount;

    bool isMouseDown;

    BezierCurve bezierCurve1;
    BezierCurve bezierCurve2;
    BezierCurve bezierCurve3;
    BezierCurve bezierCurve4;


    KunsSurface kuntzSurface;
   // LinearSurface linearSurface;
    bool withMarkers;

    private void buttonOfDataPointCounter_Click(object sender, EventArgs e)
    {
        if (dataPointCount < 12)
        {
            dataPointCount++;
            if (dataPointCount == 12)
            {
                buttonDown.Enabled = false;
                buttonUp.Enabled = false;
                buttonRight.Enabled = false;
                buttonLeft.Enabled = false;
                buttonNaNas.Enabled = false;
                buttonOtNas.Enabled = false;
            }
        }
        else
        {
            dataPointCount = 0;
            buttonDown.Enabled = true;
            buttonUp.Enabled = true;
            buttonRight.Enabled = true;
            buttonLeft.Enabled = true;
            buttonNaNas.Enabled = true;
            buttonOtNas.Enabled = true;
        }

        this.Refresh();
    }

    private void checkBoxOfMerkersEnabled_CheckedChanged(object sender, EventArgs e)
    {
        if (checkBoxOfMerkersEnabled.Checked) checkBoxOfMerkersEnabled.BackColor = Color.GreenYellow;
        else checkBoxOfMerkersEnabled.BackColor = System.Drawing.SystemColors.ButtonHighlight;

        panelOfApproximation.Enabled = checkBoxOfMerkersEnabled.Checked;
        panelOfDataPoints.Enabled = checkBoxOfMerkersEnabled.Checked;
        withMarkers = checkBoxOfMerkersEnabled.Checked;
        this.Refresh();
    }

    private void isKuns_CheckedChanged(object sender, EventArgs e)
    {
        if (isKunsCheckBox.BackColor == System.Drawing.SystemColors.Highlight)
        {
            isKunsCheckBox.Location = new System.Drawing.Point(12, 51);
            isKunsCheckBox.Text = "Лин. поверхность Кунса";
            isKunsCheckBox.BackColor = Color.Red;
        }
        else
        {
       
            isKunsCheckBox.Text = "Обычная лин. поверхность";
            isKunsCheckBox.BackColor = System.Drawing.SystemColors.Highlight;
        }
        this.Refresh();
    }
}
    