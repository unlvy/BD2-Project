CREATE PROC init_points_table AS
BEGIN
	CREATE TABLE Points (ID int IDENTITY(1,1) PRIMARY KEY, point dbo.Point)
END;
GO

CREATE PROC init_polygons_table AS
BEGIN
	CREATE TABLE Polygons (id INT IDENTITY(1,1) PRIMARY KEY, polygon dbo.Polygon);
END;
GO

CREATE PROC delete_points_table AS
BEGIN
	DROP TABLE Points;
END;
GO

CREATE PROC delete_polygons_table AS
BEGIN
	DROP TABLE Polygons;
END;
GO