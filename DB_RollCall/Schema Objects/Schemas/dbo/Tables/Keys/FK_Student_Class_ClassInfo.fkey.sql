ALTER TABLE [dbo].[Student_Class]
    ADD CONSTRAINT [FK_Student_Class_ClassInfo] FOREIGN KEY ([ClassID]) REFERENCES [dbo].[ClassInfo] ([ID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

