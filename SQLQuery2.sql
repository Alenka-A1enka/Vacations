use testdb

DECLARE @department INT;
SET @department = (SELECT subdivision_id FROM collaborators WHERE name='Сотрудник 1');


WITH Recursive (id, parent_id)
AS
(
    SELECT id, parent_id
    FROM subdivisions s
    WHERE s.id = @department
    UNION ALL
    SELECT s.id, s.parent_id
    FROM subdivisions s
        JOIN Recursive r ON s.parent_id = r.id
)

SELECT * FROM collaborators WHERE subdivision_id IN
(SELECT id FROM Recursive WHERE parent_id IS NOT NULL AND id <> 100055 and id <> 100059)
AND 
age < 40 and len(name) > 11