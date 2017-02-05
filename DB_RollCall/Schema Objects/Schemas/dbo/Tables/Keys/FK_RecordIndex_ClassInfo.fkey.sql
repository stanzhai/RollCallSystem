ALTER TABLE [dbo].[RecordIndex]
    ADD CONSTRAINT [FK_RecordIndex_ClassInfo] FOREIGN KEY ([ClassID]) REFERENCES [dbo].[ClassInfo] ([ID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

