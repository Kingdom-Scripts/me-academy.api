﻿using FluentValidation;

namespace me_academy.core.Models.Input.Series;

public class SeriesCourseModel
{
    //public required string CourseUid { get; set; }
    public int Order { get; set; }
}

public class SeriesCourseModelValidator : AbstractValidator<SeriesCourseModel>
{
    public SeriesCourseModelValidator()
    {

    }
}
