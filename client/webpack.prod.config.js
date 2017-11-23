const { resolve, join } = require("path");
const webpack = require("webpack");
const SharedConfig = require("./webpack.shared.config.js");
const ExtractTextPlugin = require('extract-text-webpack-plugin');


module.exports = {
  ...SharedConfig,
  entry: [
    'babel-polyfill',
    ...SharedConfig.entry
  ],
  output: {
    filename: "bundle.js",
    path: resolve(__dirname, "../server/wwwroot"),
    publicPath: "/"
  },
  devtool: "source-map",
  plugins: [
    ...SharedConfig.plugins,
    new ExtractTextPlugin({ filename: 'bundle.css', allChunks: false }),
    new webpack.DefinePlugin({
      __DEV__: false
    })
  ]
};
