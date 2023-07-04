    
while (1)    
fwrite(tcpipClient,'READY');
rawData=fread(tcpipClient);
    rawData_uint = uint8(rawData);                      %// cast them to "uint8" if they are not already
    rawData_float = typecast( fliplr(rawData_uint) , 'single');
    raw_Data_double = double(rawData_float);

    R = raw_Data_double(1:3:end-1);
    R = reshape(R, NrPixel_x, NrPixel_y);
    R = R';
    amplitude = R(end:-1:1,:);
    imagesc(R)
    pause(0.1)
    disp('next image')
end