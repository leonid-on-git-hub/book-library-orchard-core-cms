'use strict';
var gulp = require('./node_modules/gulp/index.js');
var sass = require('./node_modules/gulp-sass/index.js')(require('sass'));
var cleanCss = require('./node_modules/gulp-clean-css/index.js');
var rename = require('./node_modules/gulp-rename/index.js');
var buildConfig = {
	distFolder: './wwwroot/css'
};
gulp.task('sass', function () {
	return gulp.src('./Assets/scss/**/*.scss')
		.pipe(sass().on('error', sass.logError))
		.pipe(gulp.dest(buildConfig.distFolder))
		.pipe(cleanCss())
		.pipe(rename({
			suffix: '.min'
		}))
		.pipe(gulp.dest(buildConfig.distFolder));
});
gulp.task('sass:watch', function () {
	gulp.watch('./Assets/scss/**/*.scss', gulp.series('sass'));
});