% function [depth_estimate,pointcloud] = depth_estimator(amplitude, depth, mod_f, fov, noiselevel, amplitude_treshhold)
function [depth_estimate] = depth_estimator(amplitude, depth, mod_f, noiselevel, amplitude_treshhold, amplitude_saturation_treshhold)

NrPixel_sr_x = size(amplitude,1); 
NrPixel_sr_y = size(amplitude,2);

c0 = 3e8; 
lambda = c0/mod_f;

noise = noiselevel/2+ noiselevel*randn(NrPixel_sr_x, NrPixel_sr_y);
phase = 2 * pi * (depth ./ (lambda/2));
phase = mod(phase,2*pi);

amplitude_add_noise = amplitude + noise;

[amplitude_res, phase_res] = super_resolution(amplitude_add_noise, phase, NrPixel_sr_x, NrPixel_sr_y);
% amplitude_res = amplitude_noise;
% phase_res = phase;
NrPixel_x = size(amplitude_res,1);
NrPixel_y = size(amplitude_res,2);
old = amplitude_res;
amplitude_res = imgaussfilt(amplitude_res,0.46,'FilterDomain','frequency');


amp_index = find(amplitude_res< amplitude_treshhold);
amp_index_saturation = find(amplitude_res > amplitude_saturation_treshhold);
mask = ones(NrPixel_x, NrPixel_y);
mask(amp_index) = 0;
mask(amp_index_saturation) = 0;

I1 = amplitude_res  .* sin(phase_res);
I2 = amplitude_res  .* sin(phase_res + pi/2);
I3 = amplitude_res  .* sin(phase_res + pi);
I4 = amplitude_res  .* sin(phase_res + (3*pi)/2);

phase_estimate = atan2((I3-I1),(I4-I2)) + pi;
depth_estimate = (c0 .*  phase_estimate) ./ (4 * pi * mod_f);

depth_estimate = depth_estimate.*mask;

% fov_rad = fov * pi / 180;        
% fx = (NrPixel_x / 2) / tan(fov_rad / 2);                                
% fy = (NrPixel_y / 2) / tan(fov_rad / 2);
% cx = NrPixel_x/2; 
% cy = NrPixel_y/2;

% pointcloud = computePointCloud(depth_estimate, fx, fy, cx, cy, NrPixels_x, NrPixels_y);

