'use strict';
var config = {
	targetFolder: './wwwroot/css'
};
var gulp = require('./node_modules/gulp/index.js');
var sass = require('./node_modules/gulp-sass/index.js')(require('sass'));
var cleanCss = require('./node_modules/gulp-clean-css/index.js');
var rename = require('./node_modules/gulp-rename/index.js');
gulp.task('sass', function () {
	return gulp.src('./Assets/scss/**/*.scss')
		.pipe(sass().on('error', sass.logError))
		.pipe(gulp.dest(config.targetFolder))
		.pipe(cleanCss())
		.pipe(rename({
			suffix: '.min'
		}))
		.pipe(gulp.dest(config.targetFolder));
});
gulp.task('sass:watch', function () {
	gulp.watch('./Assets/scss/**/*.scss', gulp.series('sass'));
});