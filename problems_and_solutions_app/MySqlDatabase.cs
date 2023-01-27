using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace problems_and_solutions_app
{
    public class MySqlDatabase
    {

        private const string connectionStr =
                "server=localhost;" +
                "user id=root;" +
                "password=53zWm5LLUYpF6J;" +
                "database=problems_solutions_db;";

        public bool IsValidUserCredentials(string username, string password)
        {
            using (var connection = new MySqlConnection(connectionStr))
            {
                connection.Open();
                using (MySqlCommand command = new MySqlCommand("SELECT ValidateUserCredentials(@username, @user_password)", connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@username", username);
                    command.Parameters.AddWithValue("@user_password", password);
                    var result = command.ExecuteScalar();
                    return Convert.ToBoolean(result);
                }
            }


        }


        public bool UserExists(string username)
        {
            using (var connection = new MySqlConnection(connectionStr))
            {
                connection.Open();
                using (MySqlCommand command = new MySqlCommand("SELECT UserExists(@username)", connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@username", username);
                    var result = command.ExecuteScalar();
                    return Convert.ToBoolean(result);
                }
            }


        }


        public void AddUser(string username, string email, string password)
        {
            using (var connection = new MySqlConnection(connectionStr))
            {
                connection.Open();
                using (MySqlCommand command = new MySqlCommand("AddUser", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new MySqlParameter("$username", MySqlDbType.VarChar)).Value = username;
                    command.Parameters.Add(new MySqlParameter("$email", MySqlDbType.VarChar)).Value = email;
                    command.Parameters.Add(new MySqlParameter("$user_password", MySqlDbType.VarChar)).Value = password;
                    command.ExecuteNonQuery();
                }
            }

        }

        public void AddProblem(string problemStatement, string username)
        {
            using (var connection = new MySqlConnection(connectionStr))
            {
                connection.Open();
                using (MySqlCommand command = new MySqlCommand("AddProblem", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new MySqlParameter("creator_name", MySqlDbType.VarChar)).Value = username;
                    command.Parameters.Add(new MySqlParameter("problem_statement", MySqlDbType.Text)).Value = problemStatement;
                    command.ExecuteNonQuery();
                }
            }
        }


        public void AddSolution(string solutionText, int problemId, string username)
        {
            using (var connection = new MySqlConnection(connectionStr))
            {
                connection.Open();
                using (MySqlCommand command = new MySqlCommand("AddSolution", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new MySqlParameter("problem_id", MySqlDbType.Int64)).Value = problemId;
                    command.Parameters.Add(new MySqlParameter("solution_text", MySqlDbType.Text)).Value = solutionText;
                    command.Parameters.Add(new MySqlParameter("creator_name", MySqlDbType.VarChar)).Value = username;
                    command.ExecuteNonQuery();
                }
            }
        }


        public void DeleteSolution(int solutionId)
        {
            using (var connection = new MySqlConnection(connectionStr))
            {
                connection.Open();
                using (MySqlCommand command = new MySqlCommand("DeleteSolution", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new MySqlParameter("solution_id", MySqlDbType.Int64)).Value = solutionId;

                    command.ExecuteNonQuery();
                }
            }
        }


        public void DeleteProblem(int problemId)
        {
            using (var connection = new MySqlConnection(connectionStr))
            {
                connection.Open();
                using (MySqlCommand command = new MySqlCommand("DeleteProblem", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new MySqlParameter("$problem_id", MySqlDbType.Int64)).Value = problemId;
                    command.ExecuteNonQuery();
                }
            }
        }

        public List<Problem> GetProblems(string username)
        {
            var problems = new List<Problem>();
            using (var connection = new MySqlConnection(connectionStr))
            {
                connection.Open();
                var cmd = new MySqlCommand("GetProblemsByUsername", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("username", username);

                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var problem = new Problem
                    {
                        Id = reader.GetInt32("problem_id"),
                        ProblemStatement = reader.GetString("problem_statement")
                    };
                    problems.Add(problem);
                }
            }
            return problems;
        }

        public List<Problem> GetProblems()
        {
            var problems = new List<Problem>();
            using (var connection = new MySqlConnection(connectionStr))
            {
                connection.Open();
                var cmd = new MySqlCommand("GetProblems", connection);
                cmd.CommandType = CommandType.StoredProcedure;

                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var problem = new Problem
                    {
                        Id = reader.GetInt32("problem_id"),
                        ProblemStatement = reader.GetString("problem_statement")
                    };
                    problems.Add(problem);
                }
            }
            return problems;
        }

        public List<Problem> GetProblemsLike(string searchText)
        {
            var problems = new List<Problem>();
            using (var connection = new MySqlConnection(connectionStr))
            {
                connection.Open();
                var cmd = new MySqlCommand("SearchProblems", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new MySqlParameter("search_text", MySqlDbType.Text)).Value = searchText;

                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var problem = new Problem
                    {
                        Id = reader.GetInt32("problem_id"),
                        ProblemStatement = reader.GetString("problem_statement")
                    };
                    problems.Add(problem);
                }
            }
            return problems;
        }

        public List<Problem> GetProblemsByUsernameLike(string searchText, string username)
        {
            var problems = new List<Problem>();
            using (var connection = new MySqlConnection(connectionStr))
            {
                connection.Open();
                var cmd = new MySqlCommand("SearchProblemsByUsername", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new MySqlParameter("search_text", MySqlDbType.Text)).Value = searchText;
                cmd.Parameters.Add(new MySqlParameter("$creator_name", MySqlDbType.VarChar)).Value = username;

                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var problem = new Problem
                    {
                        Id = reader.GetInt32("problem_id"),
                        ProblemStatement = reader.GetString("problem_statement")
                    };
                    problems.Add(problem);
                }
            }
            return problems;
        }


        public List<Solution> GetSolutions(int problemId)
        {
            var solutions = new List<Solution>();
            using (var connection = new MySqlConnection(connectionStr))
            {
                connection.Open();
                var cmd = new MySqlCommand("GetSolutionsByProblemId", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("problem_id", problemId);
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var solution = new Solution
                    {
                        Id = reader.GetInt32("solution_id"),
                        SolutionText = reader.GetString("solution_text")
                    };
                    solutions.Add(solution);
                }
            }
            return solutions;
        }

        public void UpdateProblemText(int problemId, string newProblemText)
        {             
            using (var connection = new MySqlConnection(connectionStr))
            {
                connection.Open();
                using (MySqlCommand command = new MySqlCommand("UpdateProblem", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new MySqlParameter("$problem_statement", MySqlDbType.Text)).Value = newProblemText;
                    command.Parameters.Add(new MySqlParameter("$problem_id", MySqlDbType.Int64)).Value = problemId;
                    command.ExecuteNonQuery();
                }
            }
        }



    }




}
