﻿using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CsvHelper;
using System.Globalization;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using System.Windows.Forms.DataVisualization.Charting;


namespace StockAnalyzer
{

    public enum Patterns
    {
        Doji,
        Marubozu_bullish,
        Marubozu_bearish,
        Hammer_bullish,
        Hammer_bearish
    }
    public partial class Form1 : Form
    {
        DataTable stockValues = new DataTable(); // datatable to hold stock value for display
        CandlestickReader candlestickReader;
        Patterns highlightPattern;
        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Adds items in a specified directory to a specified ComboBox
        /// </summary>
        /// <param name="comboBox"></param>
        /// <param name="dataFolder"></param>
        /// <param name="format"></param>
        private void LoadComboBoxItems(ComboBox comboBox, string dataFolder, string format) 
        {
            comboBox.Items.Clear();
            string directoryPath = dataFolder;
            if (Directory.Exists(directoryPath))
            {
                string[] fileNames = Directory.GetFiles(directoryPath, format);
                foreach (string fileName in fileNames)
                {
                    comboBox.Items.Add(Path.GetFileName(fileName));
                }
            }
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            string dataFolder = "Stock Data";
            highlightPattern = Patterns.Doji;
            LoadComboBoxItems(comboBoxTickerSelect, dataFolder, "*-Day.csv");
        }

        private void buttonLoad_Click(object sender, EventArgs e)
        {
            string dataFolder = "Stock Data"; // Folder containing stock data
            string timePeriod = "Day"; // selected time period
            DateTime startDate = dateTimePickerStartDate.Value;
            DateTime endDate = dateTimePickerEndDate.Value;

            if (radioButtonDaily.Checked)
            {
                timePeriod = "Day";
            }
            else if (radioButtonWeekly.Checked)
            {
                timePeriod = "Week";
            }
            else if (radioButtonMonthly.Checked)
            {
                timePeriod = "Month";
            }

            string tickerName = comboBoxTickerSelect.Text; // gets text from combobox for ticker
            StockChart displayChart = new StockChart(dataFolder, tickerName, timePeriod, startDate, endDate, highlightPattern);
            displayChart.Show();
            this.candlestickReader = new CandlestickReader(startDate, endDate, displayChart.getFilePath());
        }

        private void radioButtonDaily_CheckedChanged(object sender, EventArgs e)
        {
            string dataFolder = "Stock Data"; // Folder containing stock data
            LoadComboBoxItems(comboBoxTickerSelect, dataFolder, "*-Day.csv");
        }

        private void radioButtonWeekly_CheckedChanged(object sender, EventArgs e)
        {
            string dataFolder = "Stock Data"; // Folder containing stock data
            LoadComboBoxItems(comboBoxTickerSelect, dataFolder, "*-Week.csv");
        }

        private void radioButtonMonthly_CheckedChanged(object sender, EventArgs e)
        {
            string dataFolder = "Stock Data"; // Folder containing stock data
            LoadComboBoxItems(comboBoxTickerSelect, dataFolder, "*-Month.csv");
        }
        public long countLines(FileInfo file) /// Reads in a FileInfo object and returns the number of lines in the file
        {
            long lineCount = 0;
            using (StreamReader reader = new StreamReader(file.FullName))
            {
                while (reader.ReadLine() != null)
                {
                    lineCount++;
                }
            }

            return lineCount;
        }

        private void labelEndDate_Click(object sender, EventArgs e)
        {

        }

        private void comboBoxTickerSelect_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void radioButtonDoji_CheckedChanged(object sender, EventArgs e)
        {
            highlightPattern = Patterns.Doji;
        }

        private void radioButtonMarubozuBullish_CheckedChanged(object sender, EventArgs e)
        {
            highlightPattern = Patterns.Marubozu_bullish;
        }

        private void radioButtonMarubozuBearish_CheckedChanged(object sender, EventArgs e)
        {
            highlightPattern = Patterns.Marubozu_bearish;
        }

        private void radioButtonHammerBearish_CheckedChanged(object sender, EventArgs e)
        {
            highlightPattern = Patterns.Hammer_bearish;
        }

        private void radioButtonHammerBullish_CheckedChanged(object sender, EventArgs e)
        {
            highlightPattern = Patterns.Hammer_bullish;
        }
    }
}
