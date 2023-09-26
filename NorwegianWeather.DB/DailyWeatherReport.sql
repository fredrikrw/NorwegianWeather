CREATE TABLE [dbo].[DailyWeatherReport]
(
	[CityName] VARCHAR(200) NOT NULL,
	[Date] DATETIME2 NOT NULL,
	[TemperatureMax] FLOAT NOT NULL,
	[TemperatureAverage] FLOAT NOT NULL,
	[TemperatureMin] FLOAT NOT NULL,
	[CloudCoverAverage] FLOAT NOT NULL,
	[Percipitation] FLOAT NOT NULL,
	[WindSpeedAverage] FLOAT NOT NULL,
	[TemperatureUnit] INT NOT NULL,
	PRIMARY KEY ([CityName], [Date])
)
