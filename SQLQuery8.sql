CREATE TABLE [dbo].[request] (
    [id]             INT            IDENTITY (1, 1) NOT NULL,
    [ip_adress]      VARBINARY (16) NOT NULL,
    [ip_id]          INT            NULL,
    [date_r]         DATE           NOT NULL,
    [method_r]       VARCHAR (255)  NOT NULL,
    [file_r_path]    VARCHAR (255)  NOT NULL,
    [file_r_path_id] INT            NULL,
    [status_r]       VARCHAR (255)  NOT NULL,
    [value_r]        VARCHAR (255)  NOT NULL,
    PRIMARY KEY CLUSTERED ([id] ASC)
);