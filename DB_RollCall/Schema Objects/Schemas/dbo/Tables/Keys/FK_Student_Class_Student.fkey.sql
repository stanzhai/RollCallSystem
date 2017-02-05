ALTER TABLE [dbo].[Student_Class]
    ADD CONSTRAINT [FK_Student_Class_Student] FOREIGN KEY ([StudentNo]) REFERENCES [dbo].[Student] ([No]) ON DELETE NO ACTION ON UPDATE NO ACTION;

