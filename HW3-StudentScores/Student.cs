using System.Collections.Generic;

namespace HW3_StudentScores
{
    /// <summary>
    /// Student class - Student object which holds their own name, 
    /// totalAssignments they have, current letter grade, and a dictionary using
    /// Assignment# as a key, and the assignmentScore as the value.
    /// </summary>
    public class Student
    {

        #region Attributes      
        /// <summary>
        /// Student Name
        /// </summary>
        private string name; 

        /// <summary>
        /// Total number of assignments for an individual student
        /// </summary>
        private int totalAssignments;

        /// <summary>
        /// Holds the current letter grade - Calculated by using CalculateGrade() function
        /// </summary>
        private string letterGrade;

        /// <summary>
        /// Holds all of the students grades - Key: Assignment#, Value: Score
        /// </summary>
        public Dictionary<int, double> grades = new Dictionary<int, double>();
        #endregion

        #region Properties
        /// <summary>
        /// Property for getting/setting student name
        /// </summary>
        public string studentName
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }

        public string studentGrade
        {
            get
            {
                return letterGrade;
            }
        }
        #endregion

        #region Getter/Setter
        /// <summary>
        /// Sets the score of a specific assignment number
        /// </summary>
        /// <param name="assignment"></param>
        /// <param name="score"></param>
        public void SetGrade(int assignment, double score)
        {
            grades[assignment] = score;
        }

        /// <summary>
        /// Gets the score of a specfic assignment number
        /// </summary>
        /// <param name="assignment"></param>
        /// <returns></returns>
        public double getGrade(int assignment)
        {
            return grades[assignment];
        }
        #endregion

        #region Constructor
        public Student(string name, int numAssignments)
        {
            this.name = name;
            totalAssignments = numAssignments;
            CreateAssignments(totalAssignments);
        }
        #endregion

        #region Functions
        /// <summary>
        /// Based on the number of assignments, creates all grades
        /// with the default score being 0.
        /// </summary>
        /// <param name="numberOfAssignments"></param>
        public void CreateAssignments(int numberOfAssignments)
        {
            for (int i = 0; i < numberOfAssignments; i++)
            {
                grades.Add(i, 0);
            }
            CalculateGrade();
        }

        public double CalculateGrade()
        {
            double points = 0;
            for (int i = 0; i < totalAssignments; i++)
            {
                points = points + grades[i];
            }
            double grade = (points / (totalAssignments * 100)) * 100;
            letterGrade = CalculateLetterGrade(grade);
            return grade;
        }

        public string CalculateLetterGrade(double grade)
        {
            if (grade >= 93)
                return "A";
            else if (grade >= 90)
                return "A-";
            else if (grade >= 87)
                return "B+";
            else if (grade >= 83)
                return "B";
            else if (grade >= 80)
                return "B-";
            else if (grade >= 77)
                return "C+";
            else if (grade >= 73)
                return "C";
            else if (grade >= 70)
                return "C-";
            else if (grade >= 67)
                return "D+";
            else if (grade >= 63)
                return "D";
            else if (grade >= 60)
                return "D-";
            else
                return "E";
        }
        #endregion

    }
}
