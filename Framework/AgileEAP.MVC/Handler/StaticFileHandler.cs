////------------------------------------------------------------------------------
//// <copyright file="StaticFileHandler.cs" company="Microsoft">
////     Copyright (c) Microsoft Corporation.  All rights reserved.
//// </copyright>
////-----------------------------------------------------------------------------

///*
// * Static File Handler
// *
// * Copyright (c) 1999 Microsoft Corporation
// */

//namespace System.Web
//{
//    using System;
//    using System.Text;
//    using System.Runtime.Serialization.Formatters;
//    using System.Threading;
//    using System.Runtime.InteropServices;
//    using System.Security;
//    using System.IO;
//    using Microsoft.Win32;
//    using System.Collections;
//    using System.Web.Hosting;
//    using System.Web.Util;
//    using System.Globalization;

//    internal static class HttpStatus
//    {
//        internal const int Unauthorized = 401;
//        internal const int Forbidden = 403;
//        internal const int NotFound = 404;
//    }

//    /////////////////////////////////////////////////////////////////////////////
//    /////////////////////////////////////////////////////////////////////////////
//    /////////////////////////////////////////////////////////////////////////////

//    internal class StaticFileHandler : IHttpHandler
//    {
//        private const int DEFAULT_CACHE_THRESHOLD = 256 * 1024;
//        private const int ERROR_ACCESS_DENIED = 5;

//        internal StaticFileHandler()
//        {
//        }

//        private static void CacheValidateHandler(
//                                         HttpContext context,
//                                         Object data,
//                                         ref HttpValidationStatus validationStatus)
//        {
//            if (context.Request.Headers["Range"] != null ||
//                 context.Request.RequestType.Equals("(GETSOURCE)") ||
//                 context.Request.RequestType.Equals("(HEADSOURCE)"))
//            {
//                validationStatus = HttpValidationStatus.IgnoreThisRequest;
//            }
//        }


//        /////////////////////////////////////////////////////////////////////////////
//        /////////////////////////////////////////////////////////////////////////////

//        public void ProcessRequest(HttpContext context)
//        {
//            ProcessRequestInternal(context);
//        }

//        public bool IsReusable
//        {
//            get { return true; }
//        }

//        private static void RespondUsingVirtualFile(string virtualPath, HttpResponse response)
//        {
//            Debug.Trace("StaticFileHandler", "Using VirtualPathProvider for " + virtualPath);

//            VirtualFile virtualFile = null;

//            if (HostingEnvironment.VirtualPathProvider.FileExists(virtualPath))
//            {
//                virtualFile = HostingEnvironment.VirtualPathProvider.GetFile(virtualPath);
//            }

//            if (virtualFile == null)
//            {
//                Debug.Trace("StaticFileHandler", "Virtual file " + virtualPath + " not found");
//                throw new HttpException(HttpStatus.NotFound,
//                                         SR.GetString(SR.File_does_not_exist));
//            }

//            response.WriteVirtualFile(virtualFile);
//            response.ContentType = MimeMapping.GetMimeMapping(virtualPath);
//        }

//        internal static void ProcessRequestInternal(HttpContext context)
//        {
//            FileInfo file;
//            HttpRequest request = context.Request;
//            HttpResponse response = context.Response;
//            string FileName = request.PhysicalPath;

//            // for non-default VirtualPathProvider use a different code path
//            if (!HostingEnvironment.UsingMapPathBasedVirtualPathProvider)
//            {
//                RespondUsingVirtualFile(request.FilePath, response);
//                return;
//            }

//            Util.Debug.Trace("GET", "Path = " + request.Path);
//            Util.Debug.Trace("GET", "File Name = " + FileName);

//            //
//            // Check whether the file exists
//            //

//            if (!FileUtil.FileExists(FileName))
//            {
//                throw new HttpException(HttpStatus.NotFound,
//                                         SR.GetString(SR.File_does_not_exist));
//            }

//            try
//            {
//                file = new FileInfo(FileName);
//            }
//            catch (IOException ioEx)
//            {
//                if (!HttpRuntime.HasFilePermission(FileName))
//                    throw new HttpException(HttpStatus.NotFound,
//                                             SR.GetString(SR.Error_trying_to_enumerate_files));
//                else
//                    throw new HttpException(HttpStatus.NotFound,
//                                             SR.GetString(SR.Error_trying_to_enumerate_files),
//                                             ioEx);
//            }
//            catch (SecurityException secEx)
//            {
//                if (!HttpRuntime.HasFilePermission(FileName))
//                    throw new HttpException(HttpStatus.Unauthorized,
//                                             SR.GetString(SR.File_enumerator_access_denied));
//                else
//                    throw new HttpException(HttpStatus.Unauthorized,
//                                             SR.GetString(SR.File_enumerator_access_denied),
//                                             secEx);
//            }

//            //
//            // To be consistent with IIS, we won't serve out hidden files
//            //

//            if ((((int)file.Attributes) & ((int)FileAttributes.Hidden)) != 0)
//            {
//                throw new HttpException(HttpStatus.NotFound,
//                                         SR.GetString(SR.File_is_hidden));
//            }

//            //
//            // To prevent the trailing dot problem, error out all file names with trailing dot.
//            //

//            if (FileName[FileName.Length - 1] == '.')
//            {
//                throw new HttpException(HttpStatus.NotFound,
//                                         SR.GetString(SR.File_does_not_exist));
//            }

//            //
//            // If the file is a directory, then it must not have a slash in
//            // end of it (if it does have a slash suffix, then the config file
//            // mappings are missing and we will just return 403.  Otherwise,
//            // we will redirect the client to the URL with this slash.
//            //

//            if ((((int)file.Attributes) & ((int)FileAttributes.Directory)) != 0)
//            {
//                if (StringUtil.StringEndsWith(request.Path, '/'))
//                {
//                    //
//                    // Just return 403
//                    //

//                    throw new HttpException(HttpStatus.Forbidden,
//                                             SR.GetString(SR.Missing_star_mapping));
//                }
//                else
//                {
//                    //
//                    // Redirect to a slash suffixed URL which will be
//                    // handled by the */ handler mapper
//                    //
//                    response.Redirect(request.Path + "/");
//                }
//            }
//            else
//            {
//                DateTime lastModified;
//                string strETag;

//                //
//                // Determine Last Modified Time.  We might need it soon
//                // if we encounter a Range: and If-Range header
//                //

//                lastModified = new DateTime(file.LastWriteTime.Year,
//                                               file.LastWriteTime.Month,
//                                               file.LastWriteTime.Day,
//                                               file.LastWriteTime.Hour,
//                                               file.LastWriteTime.Minute,
//                                               file.LastWriteTime.Second,
//                                               0);

//                //
//                // Because we can't set a "Last-Modified" header to any time
//                // in the future, check the last modified time and set it to
//                // DateTime.Now if it's in the future.
//                // This is to fix VSWhidbey #402323
//                if (lastModified > DateTime.Now)
//                {
//                    lastModified = DateTime.Now;
//                }

//                //
//                // Generate ETag
//                //

//                strETag = GenerateETag(context, lastModified);

//                //
//                // OK.  Send the static file out either
//                // entirely or send out the requested ranges
//                //

//                try
//                {
//                    BuildFileItemResponse(context,
//                                           FileName,
//                                           file.Length,
//                                           lastModified,
//                                           strETag);
//                }
//                catch (Exception e)
//                {
//                    //
//                    // Check for ERROR_ACCESS_DENIED and set the HTTP
//                    // status such that the auth modules do their thing
//                    //

//                    if ((e is ExternalException) && IsSecurityError(((ExternalException)e).ErrorCode))
//                    {
//                        throw new HttpException(HttpStatus.Unauthorized,
//                                                 SR.GetString(SR.Resource_access_forbidden));
//                    }
//                }

//                context.Response.Cache.SetLastModified(lastModified);

//                context.Response.Cache.SetETag(strETag);

//                //
//                // We will always set Cache-Control to public
//                //

//                context.Response.Cache.SetCacheability(HttpCacheability.Public);
//            }
//        }

//        private static void BuildFileItemResponse(HttpContext context,
//                                    string fileName,
//                                    long fileSize,
//                                    DateTime lastModifiedTime,
//                                    string strETag)
//        {
//            HttpRequest request = context.Request;
//            HttpResponse response = context.Response;
//            bool fCache = false;
//            string strRange;
//            int cbCacheThreshold = DEFAULT_CACHE_THRESHOLD;
//            bool fIsRangeRequest = false;

//            //
//            // Get the Range: header if it exists
//            //

//            strRange = request.Headers["Range"];
//            if (strRange != null)
//            {
//                if (StringUtil.StringStartsWithIgnoreCase(strRange, "bytes"))
//                {
//                    fIsRangeRequest = true;
//                }
//            }

//            //
//            // Give the range code a first chance at sending the ranges.  If
//            // the Range: header is syntactically invalid, then we will fall
//            // thru as if the Range: header was not present.
//            //

//            if (fIsRangeRequest &&
//                 !SendEntireEntity(context,
//                                    strETag,
//                                    lastModifiedTime))
//            {
//                // At this point we know that based on "If-Range"
//                // (if provided) we may not send the entire entity.

//#if  SUPPORT_HTTP_RANGE_REQUESTS
//                if ( RangeSupport.ProcessRangeRequest( context,
//                                                       strRange,
//                                                       fileName,
//                                                       fileSize ) ) {
//                    //
//                    // If ProcessRangeRequest() returned true, then it
//                    // handled the range somehow (either sending it back
//                    // with a 206 or sent a 416
//                    //
 
//                    response.Cache.SetNoServerCaching();
 
//                    return;
//                }
//#endif
//                //
//                // Fall thru.  The request is now cacheable again
//                //
//            }

//            if (fileSize <= cbCacheThreshold &&
//                 !request.RequestType.Equals("(GETSOURCE)") &&
//                 !request.RequestType.Equals("(HEADSOURCE)"))
//            {
//                fCache = true;
//            }

//            //
//            // Ask ASP to open the file contents and cache them
//            // (hence the second parameter to WriteFile())
//            //
//            if (fCache)
//                response.WriteFile(fileName, fCache);
//            else
//                response.TransmitFile(fileName);

//            //
//            // Specify content type. Use extension to do the mapping
//            //

//            response.ContentType = MimeMapping.GetMimeMapping(fileName);

//            //
//            // Static file handler supports byte ranges
//            //

//            response.AppendHeader("Accept-Ranges", "bytes");

//            //
//            // If we are caching, the instruct the ASP output cache to
//            // to cache the result.
//            //

//            if (fCache)
//            {
//                //
//                // Set a validation handler to check to avoid serving from
//                // ASP.NET output cache when Range or Translate:f
//                //


//                response.Cache.AddValidationCallback(
//                                                    new HttpCacheValidateHandler(CacheValidateHandler),
//                                                    null);

//                //
//                //
//                // We want to flush cache entry when static file has changed
//                //

//                response.AddFileDependency(fileName);

//                //
//                // Set an expires in the future.
//                //

//                response.Cache.SetExpires(DateTime.Now.AddDays(1));
//            }
//        }

//        internal /*public*/ static string GenerateETag(HttpContext context, DateTime lastModTime)
//        {
//            long appDomainFileTime;
//            long lastModFileTime;
//            StringBuilder strETag = new StringBuilder();

//            //
//            // For now, we will produce an ETag which
//            // will be invalidated when:
//            // a) The static file has changes (that's ok)
//            // b) When an apppool starts
//            //
//            appDomainFileTime = DateTime.Now.ToFileTime();

//            //
//            // Get 64-bit FILETIME stamps
//            //

//            lastModFileTime = lastModTime.ToFileTime();

//            //
//            // ETag is "<hexified last="" mod="">:<hexified create="" app="">"
//            //

//            strETag.Append("\"");
//            strETag.Append((lastModFileTime).ToString("X8", CultureInfo.InvariantCulture));
//            strETag.Append(":");
//            strETag.Append((appDomainFileTime).ToString("X8", CultureInfo.InvariantCulture));
//            strETag.Append("\"");

//            //
//            // Is this a strong ETag.  Do what IIS does to determine this.
//            // Compare the last modified time to now and if it earlier by
//            // more than 3 seconds, then it is strong.Equals( strIfRange ) )
//            //

//            if (!((DateTime.Now.ToFileTime() - lastModFileTime) > 30000000))
//            {
//                //
//                // Weak ETag
//                //

//                return "W/" + strETag.ToString();
//            }
//            else
//            {
//                //
//                // Stron ETag.  Leave as is
//                //

//                return strETag.ToString();
//            }
//        }

//        internal /*public*/ static bool SendEntireEntity(HttpContext context,
//                                             string strETag,
//                                             DateTime lastModifiedTime)
//        {
//            string strIfRange;
//            bool fEntireEntity = false;

//            //
//            // The only way we would not send the range (and instead send
//            // the entire entity) is if the If-Range header did not hold
//            //

//            //
//            // NOTE:  This routine is called by range handling code which
//            //        would have first verified that there is indeed a
//            //        Range: header in the request.
//            //

//            strIfRange = context.Request.Headers["If-Range"];
//            if (strIfRange == null)
//            {
//                return false;
//            }
//            else
//            {
//                //
//                // Is this an ETag or a Date?
//                // -- entity-tags are quoted strings, HTTP-dates are not
//                //

//                if (strIfRange[0] == '"')
//                {
//                    //
//                    // ETag
//                    //

//                    if (!CompareETags(strIfRange, strETag))
//                    {
//                        fEntireEntity = true;
//                    }
//                }
//                else
//                {
//                    //
//                    // Date
//                    //

//                    try
//                    {
//                        DateTime dt = DateTime.Parse(strIfRange, CultureInfo.InvariantCulture);
//                        int iDateCompare = DateTime.Compare(lastModifiedTime, dt);
//                        if (iDateCompare == 1)
//                        {
//                            fEntireEntity = true;
//                        }
//                    }
//                    catch
//                    {
//                        fEntireEntity = true;
//                    }
//                }
//            }

//            return fEntireEntity;
//        }

//        internal /*public*/ static bool IsSecurityError(int ErrorCode)
//        {
//            return (ErrorCode == ERROR_ACCESS_DENIED);
//        }

//        private static bool CompareETags(string strETag1,
//                                          string strETag2)
//        {
//            bool fMatch = false;

//            if (strETag1.Equals("*") || strETag2.Equals("*"))
//            {
//                fMatch = true;
//                goto Finished;
//            }

//            if (StringUtil.StringStartsWith(strETag1, "W/"))
//            {
//                strETag1 = strETag1.Substring(2);
//            }

//            if (StringUtil.StringStartsWith(strETag2, "W/"))
//            {
//                strETag2 = strETag2.Substring(2);
//            }

//            fMatch = strETag2.Equals(strETag1);

//        Finished:
//            return fMatch;
//        }
//    }
//}