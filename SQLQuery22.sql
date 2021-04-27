CREATE TABLE [dbo].[request] (
    [id]             INT           IDENTITY (1, 1) NOT NULL,
    [date_r]         DATE          NOT NULL,
    [method_r]       VARCHAR (255) NOT NULL,
    [status_r]       VARCHAR (255) NOT NULL,
    [value_r]        VARCHAR (255) NOT NULL,
    PRIMARY KEY CLUSTERED ([id] ASC)
);
