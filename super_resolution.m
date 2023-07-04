function [amplitude_res, phase_res] = super_resolution(amplitude, phase, NrPixel_x, NrPixel_y)


[x,y]=pol2cart(phase,amplitude./4);

x_pix = (x(1:2:NrPixel_x, 1:2:NrPixel_y) + x(2:2:NrPixel_x, 1:2:NrPixel_y) + x(1:2:NrPixel_x, 2:2:NrPixel_y) + x(2:2:NrPixel_x, 2:2:NrPixel_y));
y_pix = (y(1:2:NrPixel_x, 1:2:NrPixel_y) + y(2:2:NrPixel_x, 1:2:NrPixel_y) + y(1:2:NrPixel_x, 2:2:NrPixel_y) + y(2:2:NrPixel_x, 2:2:NrPixel_y));

[phase_res,amplitude_res] = cart2pol(x_pix,y_pix);





% amp_sum = (amplitude(1:2:NrPixel_x, 1:2:NrPixel_y) + amplitude(2:2:NrPixel_x, 1:2:NrPixel_y) + amplitude(1:2:NrPixel_x, 2:2:NrPixel_y) + amplitude(2:2:NrPixel_x, 2:2:NrPixel_y));
% 
% amp_sum_reshape = kron(amp_sum,ones(2));
% 
% amp_weighted = amplitude ./ amp_sum_reshape;
% 
% 
% amplitude_res = amp_sum ./ 4;
% phase_weighted = phase .* amp_weighted;
% 
% phase_res = (phase_weighted(1:2:NrPixel_x, 1:2:NrPixel_y) + phase_weighted(2:2:NrPixel_x, 1:2:NrPixel_y) + phase_weighted(1:2:NrPixel_x, 2:2:NrPixel_y) + phase_weighted(2:2:NrPixel_x, 2:2:NrPixel_y));
% 
% 
