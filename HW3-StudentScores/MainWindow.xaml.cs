using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace HW3_StudentScores
{
    /// <summary>
    /// Program which allows insertion of students and ability to modify
    /// grades for specific assignments. In addition, also is able to calculate
    /// their current percentage grade and letter grade.
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Attributes
        /// <summary>
        /// Holds total students needed for the gradebook
        /// </summary>
        private int totalStudents;

        /// <summary>
        /// Holds the total number of required assignments for the students
        /// </summary>
        private int totalAssignments;

        /// <summary>
        /// Holds all the current students 
        /// </summary>
        private Student[] students;

        /// <summary>
        /// Keeps track of the current student that is being accessed (viewed)
        /// </summary>
        private int currentStudentIndex;
        #endregion

        #region Constructor
        public MainWindow()
        {
            InitializeComponent();
        }
        #endregion

        #region Click Events
        /// <summary>
        /// When user clicks submit, parse and check if numbers are valid (If not, display error message).
        /// Create a specific amount of students and display a "Gradebook Created" message.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SubmitCountsButton_Click(object sender, RoutedEventArgs e)
        {

            InvalidNumberLabel.Visibility = Visibility.Hidden;

            bool result1 = int.TryParse(NumberStudentsTextBox.Text, out totalStudents);
            bool result2 = int.TryParse(NumberAssignmentsTextBox.Text, out totalAssignments);

            //If parsing was successful with valid numbers
            if (result1 && result2 && isBetween(totalStudents, 1, 10) && isBetween(totalAssignments, 1, 99))
            {

                //Creates array to hold specific amount of students
                students = new Student[totalStudents];

                //Creates students and places them into the array of students
                for (int i = 0; i < totalStudents; i++)
                {
                    students[i] = new Student("Student #" + (i + 1), totalAssignments);
                }

                //Sets student index position to 0 and updates Student Info section
                currentStudentIndex = 0;
                UpdateStudentInfoSection();

                //Immediately display scoreboard after inserting students. Is able to be commented out
                UpdateScoreboard();

                //Removes data typed from user
                NumberStudentsTextBox.Text = "";
                NumberAssignmentsTextBox.Text = "";

                //Updates Student Info grade section to list correct amount of totalassignments
                AssignmentNumberLabel.Content = "Enter Assignment Number (1-" + totalAssignments + "): ";

                //Displays "Gradebook Created" label for 3 seconds
                DisplayLabel(GradeCreatedLabel);

            }
            else
            {
                //If parsing or numbers were unsuccessful/invalid, display error label
                InvalidNumberLabel.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// Check if list of students actually exist, updates the currentStudentIndex 
        /// based on the button that was clicked, and update the student info to 
        /// reflect the current student being viewed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Navigate_Click(object sender, RoutedEventArgs e)
        {
            if (students != null && students.Length > 0)
            {
                Button btn = sender as Button;
                if (btn != null)
                {
                    switch (btn.Name)
                    {
                        case "FirstStudentButton":
                            currentStudentIndex = 0;
                            break;
                        case "PrevStudentButton":
                            if (currentStudentIndex > 0)
                                currentStudentIndex--;
                            break;
                        case "NextStudentButton":
                            if (currentStudentIndex < students.Length - 1)
                                currentStudentIndex++;
                            break;
                        case "LastStudentButton":
                            currentStudentIndex = students.Length - 1;
                            break;
                    }
                    UpdateStudentInfoSection();
                }
            }
        }

        /// <summary>
        /// Updates the current student's name
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveNameButton_Click(object sender, RoutedEventArgs e)
        {
            if (students != null && StudentNameTextBox.Text != "")
            {
                students[currentStudentIndex].studentName = StudentNameTextBox.Text;
                DisplayLabel(NameUpdatedLabel);
                UpdateStudentInfoSection();
                UpdateScoreboard();
            }
        }

        /// <summary>
        /// Parses and checks if numbers entered are valid. If valid, updates the current student
        /// assignment's grade and displays a "Score Updated" message
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveScoreButton_Click(object sender, RoutedEventArgs e)
        {
            if (students != null)
            {
                InvalidUpdateLabel.Visibility = Visibility.Hidden;

                int assignmentNumber;
                double assignmentScore;

                bool result1 = Int32.TryParse(AssignmentNumberTextBox.Text, out assignmentNumber);
                bool result2 = Double.TryParse(AssignmentScoreTextBox.Text, out assignmentScore);

                //If parsing is correct and assignment number is valid
                if (result1 && result2 && isBetween(assignmentNumber, 1, totalAssignments) && isBetween(assignmentScore, 0, 1000))
                {
                    //Updates the grade for a specific assignment
                    students[currentStudentIndex].SetGrade(assignmentNumber - 1, assignmentScore);

                    //Remove data that was entered by user
                    AssignmentNumberTextBox.Text = "";
                    AssignmentScoreTextBox.Text = "";

                    //Display "Score Updated" label
                    DisplayLabel(ScoreUpdatedLabel);

                    UpdateScoreboard();

                }
                else
                {
                    InvalidUpdateLabel.Visibility = Visibility.Visible;
                }
            }
        }

        /// <summary>
        /// On the Display Score button click event, fires DisplayScores method
        /// that redraws the Names textbox and Scores textbox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DisplayScoresButton_Click(object sender, RoutedEventArgs e)
        {
            if (students != null)
            {
                UpdateScoreboard();
            }
        }

        /// <summary>
        /// Output to File - Creates async dispatcher which calls the FileEditor class
        /// to create the file and output the data to the file created. After the data finishes
        /// writing, the correct label is displayed to notify the user if outputting to the file
        /// was successful.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OutputToFile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(Scores.Text) && !string.IsNullOrEmpty(FileNameLabel.Text))
                {
                    OutputToFileButton.IsEnabled = false;
                    WritingToFileLabel.Visibility = Visibility.Visible;

                    Dispatcher.Invoke(async () =>
                    {
                        bool result = FileEditor.OutputToFile(FileNameLabel.Text, Scores.Text);
                        await Task.Delay(TimeSpan.FromSeconds(2));

                        OutputToFileButton.IsEnabled = true;
                        WritingToFileLabel.Visibility = Visibility.Hidden;

                        if (result)
                        {
                            DisplayLabel(OutputSuccessfulLabel);
                        }
                        else
                        {
                            DisplayLabel(OutputUnsuccessfulLabel);
                        }
                    });

                }
            }
            catch (Exception ex)
            {
                ErrorHandling.HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }


        /// <summary>
        /// Resets Everything - Removes students, number of students & assignments,
        /// and clears out the textboxes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ResetScoresButton_Click(object sender, RoutedEventArgs e)
        {
            if (students != null)
            {
                totalAssignments = 0;
                totalStudents = 0;

                students = null;

                currentStudentIndex = 0;

                StudentNameLabel.Content = "";
                StudentNameTextBox.Text = "";

                Scores.Text = "";
            }

        }
        #endregion

        #region Methods
        /// <summary>
        /// Updates the Student Info section based on the current student index
        /// </summary>
        public void UpdateStudentInfoSection()
        {
            if (students != null)
            {
                StudentNameLabel.Content = students[currentStudentIndex].studentName;
                StudentNameTextBox.Text = students[currentStudentIndex].studentName;
            }
            else
            {
                StudentNameLabel.Content = "";
                StudentNameTextBox.Text = "";
            }
        }

        /// <summary>
        /// Makes a specific label visible for 3 seconds
        /// </summary>
        /// <param name="lbl"></param>
        public void DisplayLabel(Label lbl)
        {
            Dispatcher.Invoke(async () =>
            {
                lbl.Visibility = Visibility.Visible;
                await Task.Delay(TimeSpan.FromSeconds(2));
                lbl.Visibility = Visibility.Hidden;
            });
        }

        /// <summary>
        /// Populates the list of students and grades
        /// </summary>
        public void UpdateScoreboard()
        {
            //Displays all assignments, AVG, and GRADE columns
            Scores.Text = "STUDENT\t";
            for (int i = 0; i < totalAssignments; i++)
            {
                Scores.AppendText("#" + string.Format("{0:00}", i + 1) + "\t");
            }
            Scores.AppendText("AVG\tGRADE\n");

            //Displays student grade information
            for (int i = 0; i < students.Length; i++)
            {
                Scores.AppendText(students[i].studentName + "\t");
                for (int j = 0; j < totalAssignments; j++)
                {
                    Scores.AppendText(students[i].getGrade(j).ToString() + "\t");
                }
                Scores.AppendText(students[i].CalculateGrade().ToString("0.##") + "%\t" + students[i].studentGrade + "\n");
            }
        }
        #endregion

        #region Helper Function
        /// <summary>
        /// Simply determines whether a number is between a certain range - Inclusive of end bounds
        /// </summary>
        /// <param name="num"></param>
        /// <param name="low"></param>
        /// <param name="high"></param>
        /// <returns></returns>
        public static bool isBetween(int num, int low, int high)
        {
            return (num >= low && num <= high);
        }

        public static bool isBetween(double num, int low, int high)
        {
            return (num >= low && num <= high);
        }
        #endregion
    }
}
