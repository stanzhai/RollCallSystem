ALTER TABLE [dbo].[Record]
    ADD CONSTRAINT [FK_Record_Student] FOREIGN KEY ([StudentNo]) REFERENCES [dbo].[Student] ([No]) ON DELETE NO ACTION ON UPDATE NO ACTION;

