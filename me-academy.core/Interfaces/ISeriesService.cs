﻿using me_academy.core.Models.Input.Series;
using me_academy.core.Models.Input.Videos;
using me_academy.core.Models.Utilities;

namespace me_academy.core.Interfaces;

public interface ISeriesService
{
    Task<Result> CreateSeries(SeriesModel model);
    Task<Result> UpdateSeries(string seriesUid, SeriesModel model);
    Task<Result> DeleteSeries(string seriesUid);
    Task<Result> GetSeries(string seriesUid);
    Task<Result> ListSeries(SeriesSearchModel request);
    Task<Result> AddSeriesView(string seriesUid);
    Task<Result> PublishSeries(string seriesUid);
    Task<Result> ActivateSeries(string seriesUid);
    Task<Result> DeactivateSeries(string seriesUid);
    Task<Result> GetUploadToken(string seriesUid);
    Task<Result> SetPreviewDetails(string seriesUid, VideoDetailModel model);
    Task<Result> AddExistingCourseToSeries(string seriesUid, string courseUid, SeriesCourseModel model);
    Task<Result> AddNewCourseToSeries(string seriesUid, SeriesNewCourseModel model);
    Task<Result> RemoveCourseFromSeries(string seriesUid, int seriesCourseId);
}
