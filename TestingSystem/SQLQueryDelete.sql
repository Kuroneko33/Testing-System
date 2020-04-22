DELETE FROM Answers;
dbcc checkident (Answers, reseed, 0);
DELETE FROM Questions;
dbcc checkident (Questions, reseed, 0);
DELETE FROM Tests;
dbcc checkident (Tests, reseed, 0);